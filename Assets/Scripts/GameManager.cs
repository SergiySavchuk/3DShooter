using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameMenuManager gameMenuManager;
    [SerializeField]
    private LineRenderer line;
    [SerializeField]
    private GameObject arrow, targetPrefab;
    [SerializeField]
    private int targetAmount = 6;
    [SerializeField]
    private float distanceToTarget = 10f;
    [SerializeField]
    private Transform player;
    [SerializeField]
    private int timeToChangeTargets = 10, targetsToWin = 10, timeToLose = 20;

    private Vector3[] targetsPos;
    private int currentTarget;
    private List<int> idList;

    private float timer, scorePerGame;
    private int targetsShooted;
    private bool gameStarted = false;

    private void Awake()
    {
        CreateTargets();
    }

    private void Start()
    {
        StartGame();
    }

    private void Update()
    {
        if (!gameStarted) return;

        // таймери збільшуються кожного фрейму
        timer += Time.deltaTime;
        scorePerGame += Time.deltaTime;

        gameMenuManager.SetTimerText((timer + 1).ToString("f0"));
        gameMenuManager.SetTimeLeftText((timeToLose - scorePerGame).ToString("f0"));

        if (scorePerGame >= timeToLose)
            Lost();

        if (timer >= timeToChangeTargets)
        {
            ChooseNewCurrentTarget();
            SetCurrentTarget();
        }
    }

    private void CreateTargets()
    {
        // потрібен для зберігання позицій цілей, щоб малювати лінію і ставити стрілку
        targetsPos = new Vector3[targetAmount];

        // визначаю через скільки градусів будуть знаходитися цілі
        float targetDegree = 180f / (targetAmount - 1);

        for (int i = 0; i < targetAmount; i++)
        {
            // визначаю градус цілі в радіанах
            float angle = Mathf.Deg2Rad * targetDegree * i;

            // розраховую позицію цілі
            Vector3 position = new Vector3 (Mathf.Cos(angle) * distanceToTarget, 0 , Mathf.Sin(angle) * distanceToTarget);
            targetsPos[i] = position;

            // створюю ціль з префаба і розраховую, щоб вона була повернута до гравця
            GameObject target = Instantiate(targetPrefab, position, Quaternion.identity);
            target.transform.LookAt(player);
            target.transform.rotation = new Quaternion(0, target.transform.rotation.y, 0, target.transform.rotation.w);

            // зберігаю в цілі її індекс та посилання на цей скрипт
            target.GetComponent<TargetController>().SetValues(this, i);
        }
    }

    public void StartGame()
    {
        // повертаю всі значення до стандартних
        Time.timeScale = 1;
        gameStarted = true;

        timer = 0;
        scorePerGame = 0;
        targetsShooted = 0;

        // список потрібен, щоб вибирати рандомну ціль, яка б не була сусідньою з поточною
        idList = new List<int>();

        for (int i = 0; i < targetAmount; i++)
            idList.Add(i);
        
        // першу ціль вибираю з всіх наявних
        currentTarget = Random.Range(0, targetAmount);

        // зі списку видаляю поточну ціль і її сусідів
        idList.Remove(currentTarget - 1);
        idList.Remove(currentTarget);
        idList.Remove(currentTarget + 1);
        SetCurrentTarget();

        // оновлюємо данні мені до стандартних
        gameMenuManager.SetScoreText(targetsToWin.ToString());
        gameMenuManager.SetTimerText("1");
        gameMenuManager.SetTimeLeftText(timeToLose.ToString());
    }

    private void DrawLine(int from, int to)
    {
        // малюю лінію від однієї точки до іншої
        Vector3[] points = new Vector3[to - from + 1];

        // піднімаю лінію до центра цілей
        float offset = 1.5f;

        // виділяю позиції для лінії
        for (int i = from, j = 0; i <= to; i++, j++)
        {
            points[j] = new Vector3(targetsPos[i].x, targetsPos[i].y + offset, targetsPos[i].z);
        }

        // задаю точки для лінії
        line.positionCount = points.Length;
        line.SetPositions(points);
    }

    private void ChooseNewCurrentTarget() 
    {
        // потрібно для того, щоб знати, від або до якої точки малювати лінію
        int preTarget = currentTarget;

        // отримую рандомну ціль
        int tempId = Random.Range(0, idList.Count);
        int tempTarget = idList[tempId];

        // додаю поточну ціль і її сусідів, які не виходять за межі, до списку
        idList.Add(currentTarget);

        if (currentTarget - 1 >= 0)
        {
            idList.Add(currentTarget - 1);
        }

        if (currentTarget + 1 < targetAmount)
        {
            idList.Add(currentTarget + 1);
        }

        // записую поточну ціль і зі списку видаляю її і її сусідів
        currentTarget = tempTarget;
        idList.Remove(currentTarget - 1);
        idList.Remove(currentTarget);
        idList.Remove(currentTarget + 1);

        timer = 0;

        // малюю лінію від мінімального індекса до максимального
        DrawLine(Mathf.Min(preTarget, currentTarget), Mathf.Max(preTarget, currentTarget));
    }

    private void SetCurrentTarget()
    {
        // встановлюю стрілку над поточною цілью
        Vector3 position = targetsPos[currentTarget];

        float offset = arrow.transform.position.y;

        arrow.transform.position = new Vector3(position.x, offset, position.z);

        // розвертаю ціль до гравця
        arrow.transform.LookAt(player);
        arrow.transform.rotation = new Quaternion(0, arrow.transform.rotation.y, 0, arrow.transform.rotation.w);
    }

    public void TargetGotHit(int id)
    {
        // перевіряю чи гравець влучив в поточну ціль
        if (id == currentTarget)
        {
            // встановлюю нову ціль і перевіряю чи гравець виграв
            ChooseNewCurrentTarget();
            SetCurrentTarget();
            targetsShooted++;
            gameMenuManager.SetScoreText((targetsToWin - targetsShooted).ToString());
            if (targetsShooted >= targetsToWin)
            {
                Win();
            }
        }
    }

    private void Win()
    {
        // зупиняю гру
        Time.timeScale = 0;
        gameStarted = false;

        // потрібен для збереження індексу поточного результату
        int winIndex = 100;

        float[] bestScore = PlayerPrefsManager.GetBestScore(out float defaultValue);
        string[] bestScoreString = new string[10];

        List<float> bestList = new List<float>(bestScore);

        // перевіряю чи гравець побив свій рекорд
        for (int i = bestScore.Length - 1; i >= 0; i--)
        {
            if (bestScore[i] != defaultValue && scorePerGame > bestScore[i])
            {
                bestList.Insert(i + 1, scorePerGame);
                winIndex = i + 1;
                break;
            }

            // це накращий результат гравця
            if (i == 0)
            {
                bestList.Insert(i, scorePerGame);
                winIndex = i;
            }
        }

        for (int i = 0; i < bestScoreString.Length; i++)
        {
            // переводжу float до string з форматом
            if (bestScore[i] == defaultValue)
                bestScoreString[i] = "-,--";
            else
                bestScoreString[i] = bestList[i].ToString("f2");
        }

        PlayerPrefsManager.SetBestScore(bestList.ToArray());

        gameMenuManager.ShowWinPanel(bestScoreString, winIndex);
    }

    private void Lost()
    {
        Time.timeScale = 0;
        gameStarted = false;

        float[] bestScore = PlayerPrefsManager.GetBestScore(out float defaultValue);
        string[] bestScoreString = new string[10];

        for (int i = 0; i < bestScoreString.Length; i++)
        {
            // переводжу float до string з форматом
            if (bestScore[i] == defaultValue)
                bestScoreString[i] = "-,--";
            else
                bestScoreString[i] = bestScore[i].ToString("f2");
        }

        gameMenuManager.ShowLosePanel(bestScoreString);
    }

    public void GoToMainMenu()
    {
        // загружаю попередню сцену
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}
