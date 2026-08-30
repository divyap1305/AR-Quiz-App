using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class QuestionData
{
    [Header("English")]
    public string questionEnglish;
    public string optionAEnglish;
    public string optionBEnglish;
    public string optionCEnglish;
    public string optionDEnglish;

    [Header("Tamil")]
    public string questionTamil;
    public string optionATamil;
    public string optionBTamil;
    public string optionCTamil;
    public string optionDTamil;

    // 0 = A, 1 = B, 2 = C, 3 = D
    public int correctAnswer;
}

public enum QuizLanguage
{
    English,
    Tamil
}

public class QuizManager : MonoBehaviour
{
    [Header("Question UI")]
    public TMP_Text questionText;
    public TMP_Text optionAText;
    public TMP_Text optionBText;
    public TMP_Text optionCText;
    public TMP_Text optionDText;

    [Header("Score UI")]
    public TMP_Text scoreText;

    [Header("Feedback UI")]
    public GameObject feedbackPanel;
    public TMP_Text feedbackText;

    [Header("Final Score UI")]
    public GameObject finalScorePanel;
    public TMP_Text finalScoreText;
    public Button playAgainButton;

    [Header("Answer Buttons")]
    public Button optionAButton;
    public Button optionBButton;
    public Button optionCButton;
    public Button optionDButton;

    [Header("Language")]
    public TMP_Dropdown languageDropdown;

    [Header("Questions")]
    public List<QuestionData> questions = new List<QuestionData>();

    private int currentQuestionIndex = 0;
    private int score = 0;
    private bool answerSelected = false;

    private QuizLanguage currentLanguage = QuizLanguage.English;

    void Start()
    {
        feedbackPanel.SetActive(false);
        finalScorePanel.SetActive(false);

        optionAButton.onClick.AddListener(() => SelectAnswer(0));
        optionBButton.onClick.AddListener(() => SelectAnswer(1));
        optionCButton.onClick.AddListener(() => SelectAnswer(2));
        optionDButton.onClick.AddListener(() => SelectAnswer(3));

        playAgainButton.onClick.AddListener(RestartQuiz);

        if (languageDropdown != null)
        {
            languageDropdown.onValueChanged.AddListener(ChangeLanguage);
        }

        LoadQuestion();
    }

    void LoadQuestion()
    {
        feedbackPanel.SetActive(false);

        if (currentQuestionIndex >= questions.Count)
        {
            FinishQuiz();
            return;
        }

        answerSelected = false;

        QuestionData currentQuestion = questions[currentQuestionIndex];

        if (currentLanguage == QuizLanguage.English)
        {
            questionText.text = currentQuestion.questionEnglish;

            optionAText.text = currentQuestion.optionAEnglish;
            optionBText.text = currentQuestion.optionBEnglish;
            optionCText.text = currentQuestion.optionCEnglish;
            optionDText.text = currentQuestion.optionDEnglish;
        }
        else
        {
            questionText.text = currentQuestion.questionTamil;

            optionAText.text = currentQuestion.optionATamil;
            optionBText.text = currentQuestion.optionBTamil;
            optionCText.text = currentQuestion.optionCTamil;
            optionDText.text = currentQuestion.optionDTamil;
        }

        scoreText.text = "Score: " + score;

        EnableButtons();
    }

    public void ChangeLanguage(int selectedLanguage)
    {
        if (selectedLanguage == 0)
        {
            currentLanguage = QuizLanguage.English;
        }
        else if (selectedLanguage == 1)
        {
            currentLanguage = QuizLanguage.Tamil;
        }

        LoadQuestion();
    }

    public void SelectAnswer(int selectedAnswer)
    {
        if (answerSelected)
            return;

        answerSelected = true;

        QuestionData currentQuestion = questions[currentQuestionIndex];

        if (selectedAnswer == currentQuestion.correctAnswer)
        {
            score++;

            feedbackText.text = "CORRECT! +1 POINT";

            Debug.Log("Correct Answer!");
        }
        else
        {
            feedbackText.text = "WRONG!";

            Debug.Log("Wrong Answer!");
        }

        scoreText.text = "Score: " + score;

        feedbackPanel.SetActive(true);

        DisableButtons();

        StartCoroutine(NextQuestionAfterDelay());
    }

    IEnumerator NextQuestionAfterDelay()
    {
        yield return new WaitForSeconds(1.5f);

        currentQuestionIndex++;

        LoadQuestion();
    }

    void EnableButtons()
    {
        optionAButton.interactable = true;
        optionBButton.interactable = true;
        optionCButton.interactable = true;
        optionDButton.interactable = true;
    }

    void DisableButtons()
    {
        optionAButton.interactable = false;
        optionBButton.interactable = false;
        optionCButton.interactable = false;
        optionDButton.interactable = false;
    }

    void FinishQuiz()
    {
        feedbackPanel.SetActive(false);

        questionText.gameObject.SetActive(false);

        optionAButton.gameObject.SetActive(false);
        optionBButton.gameObject.SetActive(false);
        optionCButton.gameObject.SetActive(false);
        optionDButton.gameObject.SetActive(false);

        scoreText.gameObject.SetActive(false);

        finalScoreText.text =
            "QUIZ COMPLETE!\n\nYOUR SCORE\n\n" +
            score + " / " + questions.Count;

        finalScorePanel.SetActive(true);

        Debug.Log("Quiz Finished! Final Score: " + score);
    }

    void RestartQuiz()
    {
        currentQuestionIndex = 0;
        score = 0;
        answerSelected = false;

        finalScorePanel.SetActive(false);

        questionText.gameObject.SetActive(true);

        optionAButton.gameObject.SetActive(true);
        optionBButton.gameObject.SetActive(true);
        optionCButton.gameObject.SetActive(true);
        optionDButton.gameObject.SetActive(true);

        scoreText.gameObject.SetActive(true);

        LoadQuestion();
    }
}