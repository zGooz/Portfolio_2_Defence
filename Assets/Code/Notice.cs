
using UnityEngine;
using UnityEngine.UI;


public class Notice : MonoBehaviour
{
    [SerializeField] private GameObject mainObject;

    private Game main;
    private Text label;

    private void Awake()
    {
        main = mainObject.GetComponent<Game>();
        label = GetComponent<Text>();
    }

    private void OnEnable()
    {
        main.YouWinner += OnYouWinner;
        main.YouLouse += OnYouLouse;
    }

    private void OnDisable()
    {
        main.YouWinner -= OnYouWinner;
        main.YouLouse -= OnYouLouse;
    }

    private void OnYouWinner()
    {
        label.text = "You winner!";
    }

    private void OnYouLouse()
    {
        label.text = "You louse!";
    }
}
