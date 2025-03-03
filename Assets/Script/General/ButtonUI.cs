using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonUI : MonoBehaviour, IPointerEnterHandler,
    IPointerExitHandler, IPointerClickHandler, IDragHandler, IDropHandler,
    IEndDragHandler, IBeginDragHandler
{
    [Header("Debug")]
    public bool isHovered;
    public bool interactable = true;  // Added interactable property

    public Action ClickFunc = null;
    public Action MouseRightClickFunc = null;
    public Action MouseMiddleClickFunc = null;
    public Action MouseEnter = null;
    public Action FuncMouseEnter = null;
    public Action MouseExit = null;
    public Action FuncMouseExit = null;

    public Action MouseDragBegin = null;
    public Action MouseDrag = null;
    public Action MouseDragEnd = null;
    public Action MouseDrop = null;

    // Added sprites for different states
    [Header("Button Sprites")]
    public Sprite normalSprite;
    public Sprite hoverSprite;
    public Sprite clickSprite;
    public Sprite disabledSprite;

    [Header("Button Audio")]
    public string hoverAudio;
    public string clickAudio;

    private Image buttonImage;

    private void Awake()
    {
        // Get the Image component to change the button sprite
        buttonImage = GetComponent<Image>();
        buttonImage.sprite = normalSprite;
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        if (interactable)
        {
            isHovered = true;
            if (MouseEnter != null) { MouseEnter(); }
            if (FuncMouseEnter != null) { FuncMouseEnter(); }
            // Change sprite to hover state
            if (hoverSprite != null)
                buttonImage.sprite = hoverSprite;
        }
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        if (interactable)
        {
            isHovered = false;
            if (MouseExit != null) { MouseExit(); }
            if (FuncMouseExit != null) { FuncMouseExit(); }
            // Change sprite back to normal state
            buttonImage.sprite = normalSprite;
        }
    }

    public virtual void OnPointerClick(PointerEventData eventData)
    {
        if (interactable)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
                if (ClickFunc != null) ClickFunc();
            if (eventData.button == PointerEventData.InputButton.Right)
                if (MouseRightClickFunc != null) MouseRightClickFunc();
            if (eventData.button == PointerEventData.InputButton.Middle)
                if (MouseMiddleClickFunc != null) MouseMiddleClickFunc();
            // Change sprite to click state
            if (clickSprite != null)
                buttonImage.sprite = clickSprite;
        }
    }

    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        if (MouseDragBegin != null) MouseDragBegin();
    }

    public virtual void OnDrag(PointerEventData eventData)
    {
        if (MouseDrag != null) MouseDrag();
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        if (MouseDragEnd != null) MouseDragEnd();
    }

    public virtual void OnDrop(PointerEventData eventData)
    {
        if (MouseDrop != null) MouseDrop();
    }

    // Add a method to set the button to interactable or not
    public void SetInteractable(bool value)
    {
        interactable = value;
        if (interactable)
        {
            buttonImage.sprite = normalSprite;  // Reset to normal sprite when interactable
        }
        else
        {
            if (disabledSprite != null) 
                buttonImage.sprite = disabledSprite;  // Set to disabled sprite
        }
    }
}
