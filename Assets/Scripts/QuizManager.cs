using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class QuestionData
{
    public string question;
    public string optionA;
    public string optionB;
    public string optionC;
    public string optionD;

    // 0 = A, 1 = B, 2 = C, 3 = D
    public int correctAnswer;
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

    [Header("Answer Buttons")]
    public Button optionAButton;
    public Button optionBButton;
    public Button optionCButton;
    public Button optionDButton;

    [Header("Questions")]
    public List<QuestionData> questions = new List<QuestionData>();

    private int currentQuestionIndex = 0;
    private int score = 0;
    private bool answerSelected = false;

    void Start()
    {
        // Keep feedback hidden when the quiz starts
        feedbackPanel.SetActive(false);

        // Connect button clicks
        optionAButton.onClick.AddListener(() => SelectAnswer(0));
        optionBButton.onClick.AddListener(() => SelectAnswer(1));
        optionCButton.onClick.AddListener(() => SelectAnswer(2));
        optionDButton.onClick.AddListener(() => SelectAnswer(3));

        // Load first question
        LoadQuestion();
    }

    void LoadQuestion()
    {
        // Hide feedback when loading a new question
        feedbackPanel.SetActive(false);

        // Check if quiz is finished
        if (currentQuestionIndex >= questions.Count)
        {
            FinishQuiz();
            return;
        }

        answerSelected = false;

        QuestionData currentQuestion = questions[currentQuestionIndex];

        // Display question
        questionText.text = currentQuestion.question;

        // Display options
        optionAText.text = currentQuestion.optionA;
        optionBText.text = currentQuestion.optionB;
        optionCText.text = currentQuestion.optionC;
        optionDText.text = currentQuestion.optionD;

        // Display score
        scoreText.text = "Score: " + score;

        // Enable answer buttons
        EnableButtons();
    }

    void SelectAnswer(int selectedAnswer)
    {
        // Prevent selecting another answer
        // while feedback is being displayed
        if (answerSelected)
            return;

        answerSelected = true;

        QuestionData currentQuestion = questions[currentQuestionIndex];

        // Check answer
        if (selectedAnswer == currentQuestion.correctAnswer)
        {
            score++;

            feedbackText.text = "✓ CORRECT! +1 POINT";

            Debug.Log("Correct Answer!");
        }
        else
        {
            feedbackText.text = "✕ WRONG!";

            Debug.Log("Wrong Answer!");
        }

        // Update score
        scoreText.text = "Score: " + score;

        // Show feedback
        feedbackPanel.SetActive(true);

        // Disable buttons while feedback is displayed
        DisableButtons();

        // Move to next question after delay
        StartCoroutine(NextQuestionAfterDelay());
    }

    IEnumerator NextQuestionAfterDelay()
    {
        // Show feedback for 1.5 seconds
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
        // Hide feedback panel
        feedbackPanel.SetActive(false);

        // Display final message
        questionText.text = "QUIZ COMPLETE!";

        // Clear option texts
        optionAText.text = "";
        optionBText.text = "";
        optionCText.text = "";
        optionDText.text = "";

        // Disable buttons
        DisableButtons();

        // Display final score
        scoreText.text = "Final Score: " + score + " / " + questions.Count;

        Debug.Log("Quiz Finished! Final Score: " + score);
    }
}