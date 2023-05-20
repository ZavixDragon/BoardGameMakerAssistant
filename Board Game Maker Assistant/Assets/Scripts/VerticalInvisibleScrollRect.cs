using System;
using UnityEngine;

//this class assumes the following: (if you think this is too niche and specific try to code it see how difficult it is)
//view is the direct parent of content
//the scales of both rect transforms are 1
//neither rect transform is rotated
//your content is a vertical content layout
//that your content's y pivot is 1
public class VerticalInvisibleScrollRect : MonoBehaviour
{
    [SerializeField] private RectTransform content;
    [SerializeField] private RectTransform view;
    [SerializeField] private float sensitivity;

    public void Update()
    {
        var scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll == 0)
            return;
        Vector2 localMousePosition = view.InverseTransformPoint(Input.mousePosition);
        if (!view.rect.Contains(localMousePosition))
            return;
        var position = content.anchoredPosition;
        var maxY = Math.Max(0, content.rect.size.y - view.rect.size.y);
        var change = - scroll * sensitivity;
        if (position.y + change < 0)
            change = - position.y;
        if (position.y + change > maxY)
            change = maxY - position.y;
        content.anchoredPosition = new Vector2(position.x, position.y + change);
    }
}