using UnityEngine;

public class Loading : MonoBehaviour
{
    [SerializeField] private GameObject gameObject;

    private void OnEnable() => Message.Subscribe<ToggleLoading>(_ => gameObject.SetActive(!gameObject.activeInHierarchy), this);

    private void OnDisable() => Message.Unsubscribe(this);
}