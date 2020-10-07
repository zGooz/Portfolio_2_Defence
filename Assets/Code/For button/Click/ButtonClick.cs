
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;


public class ButtonClick : MonoBehaviour, IPointerClickHandler
{
    public event UnityAction Click;

    private void OnClick()
    {
        Click?.Invoke();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnClick();
    }
}
