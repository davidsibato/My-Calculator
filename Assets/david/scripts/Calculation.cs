using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Data;

public class Calculation : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI screenCalculationText;
    [SerializeField] GameObject buttonsGroupParent;

    Button[] calculatorButtons;
    string textToCalculate = "";

    private bool isLastResult = false;
    void Start()
    {
        calculatorButtons = buttonsGroupParent.GetComponentsInChildren<Button>();

        for (int i = 0; i < calculatorButtons.Length; i++)
        {
            // Adding the appropriate OnClick listener to each button
            switch (calculatorButtons[i].name)
            {
                case "clear":
                    calculatorButtons[i].onClick.AddListener(OnClearButtonClick);
                    break;
                case "=":
                    calculatorButtons[i].onClick.AddListener(OnEqualButtonClick);
                    break;
                case "CE":
                    calculatorButtons[i].onClick.AddListener(OnCEButtonClick);
                    break;

                default:
                    int num;
                    if (int.TryParse(calculatorButtons[i].name, out num))
                    {
                        int value = num;
                        calculatorButtons[i].onClick.AddListener(() => OnNumberButtonClick(value));
                    }
                    else
                    {
                        string value = calculatorButtons[i].name;
                        calculatorButtons[i].onClick.AddListener(() => OnFunctionButtonClick(value));
                    }
                    break;
            }
        }
    }

    void OnNumberButtonClick(int num)
    {
        if (isLastResult)
        {
            textToCalculate = "";
            isLastResult = false;
        }
        textToCalculate += num;
        screenCalculationText.text = textToCalculate;
    }

    void OnFunctionButtonClick(string function)
    {
        isLastResult = false;
        if (function == "÷")
        {
            textToCalculate += "/";
        }
        else if (function == "x")
        {
            textToCalculate += "*";
        }
        else if (function == "+/-")
        {
            if (textToCalculate.Length > 0)
            {
                char lastChar = textToCalculate[textToCalculate.Length - 1];
                if (lastChar == '+' || lastChar == '-' || lastChar == '*' || lastChar == '/')
                {
                    textToCalculate += "-";
                }
                else
                {
                    if (textToCalculate[0] != '-')
                    {
                        textToCalculate = "-" + textToCalculate;
                    }
                    else
                    {
                        textToCalculate = textToCalculate.Remove(0, 1);
                    }
                }
            }
        }
        else if (function == "%")
        {
            // check if last char is number
            if (char.IsDigit(textToCalculate[textToCalculate.Length - 1]))
            {
                string[] splitText = textToCalculate.Split(new[] { "+", "-", "*", "/", "%" }, System.StringSplitOptions.RemoveEmptyEntries);
                int lastNumber = int.Parse(splitText[splitText.Length - 1]);
                textToCalculate += "%" + (lastNumber / 100);
            }
            else
            {
                textToCalculate += "%";
            }
        }

        else if (function == "CE")
        {
            if (textToCalculate.Length > 0)
            {
                textToCalculate = textToCalculate.Remove(textToCalculate.Length - 1);
            }
            screenCalculationText.text = textToCalculate;
        }
        else
        {
            textToCalculate += function;
        }
        screenCalculationText.text = textToCalculate;
    }
    void OnClearButtonClick()
    {
        textToCalculate = "";
        screenCalculationText.text = textToCalculate;
    }

    void OnEqualButtonClick()
    {
        try
        {
            var result = new DataTable().Compute(textToCalculate, null);
            textToCalculate = result.ToString();
            screenCalculationText.text = textToCalculate;
            isLastResult = true;
        }
        catch (UnityEngine.UnityException ex)
        {   

            screenCalculationText.text = "Error: " + ex.Message;
            textToCalculate=screenCalculationText.text;
        }
    }

    void OnCEButtonClick()
    {
        if (textToCalculate.Length > 0)
        {
            textToCalculate = textToCalculate.Remove(textToCalculate.Length - 1);
        }
        screenCalculationText.text = textToCalculate;
    }
}