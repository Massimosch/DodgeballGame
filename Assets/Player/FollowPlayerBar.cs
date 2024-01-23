using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FollowPlayerBar : MonoBehaviour
{
    public Transform player;
    public Vector3 offset;
    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        Vector3 playerScreenPos = mainCamera.WorldToScreenPoint(player.position + offset);
        GetComponent<RectTransform>().position = playerScreenPos;
    }
}
