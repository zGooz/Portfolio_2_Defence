
using UnityEngine;
using UnityEngine.UI;


public class Notice : MonoBehaviour
{
    [SerializeField] 
    private GameObject main;
    private Game game;
    private Text label;

    private void Awake()
    {
        game = main.GetComponent<Game>();
        label = GetComponent<Text>();
    }

    private void OnEnable()
    {
        game.YouWinner += OnYouWinner;
        game.YouLouse += OnYouLouse;
    }

    private void OnDisable()
    {
        game.YouWinner -= OnYouWinner;
        game.YouLouse -= OnYouLouse;
    }

    private void OnYouWinner() { label.text = "You winner!"; }
    private void OnYouLouse() { label.text = "You louse!"; }
}
