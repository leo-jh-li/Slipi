using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    private GameManager m_gameManager;

    [Header("Touch Controls")]
    [SerializeField] private float m_minSwipeDistance;
    private Vector2 m_touchStartPos;
    private Vector2 m_touchEndPos;
    [HideInInspector] public bool onLevelCompleteScreen;

    private void Start() {
        m_gameManager = GetComponent<GameManager>();
    }

    private void Update()
    {
        if (Input.GetAxisRaw("Horizontal") > 0) {
            m_gameManager.MakeMove(Direction.RIGHT);
        } else if (Input.GetAxisRaw("Horizontal") < 0) {
            m_gameManager.MakeMove(Direction.LEFT);
        } else if (Input.GetAxisRaw("Vertical") > 0) {
            m_gameManager.MakeMove(Direction.UP);
        } else if (Input.GetAxisRaw("Vertical") < 0) {
            m_gameManager.MakeMove(Direction.DOWN);
        }
        if (Input.GetKeyDown("r")) {
            m_gameManager.ResetLevel();
        }
        if (Input.GetButton("Cancel") || (onLevelCompleteScreen && Input.GetButton("Submit"))) {
            m_gameManager.ReturnToLevelSelect();
        }

        // Touch controls
        foreach (Touch touch in Input.touches) {
            if (touch.phase == TouchPhase.Began) {
                m_touchStartPos = touch.position;
                m_touchEndPos = touch.position;
            } else if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Ended) {
                m_touchEndPos = touch.position;
                ProcessSwipe();
            }
        }
    }

    private void ProcessSwipe() {
        if (MinSwipeDistanceMet()) {
            if (IsHorizontalSwipe()) {
                Direction swipeDir = m_touchStartPos.x - m_touchEndPos.x > 0 ? Direction.LEFT : Direction.RIGHT;
                m_gameManager.MakeMove(swipeDir);

            } else {
                Direction swipeDir = m_touchStartPos.y - m_touchEndPos.y > 0 ? Direction.DOWN : Direction.UP;
                m_gameManager.MakeMove(swipeDir);
            }
            m_touchEndPos = m_touchStartPos;
        }
    }

    private bool IsHorizontalSwipe() {
        return CalculateHorizontalDistance() > CalculateVerticalDistance();
    }

    private bool MinSwipeDistanceMet() {
        return CalculateHorizontalDistance() >= m_minSwipeDistance || CalculateVerticalDistance() >= m_minSwipeDistance;
    }

    private float CalculateHorizontalDistance() {
        return Mathf.Abs(m_touchStartPos.x - m_touchEndPos.x);
    }

    private float CalculateVerticalDistance() {
        return Mathf.Abs(m_touchStartPos.y - m_touchEndPos.y);
    }
}
