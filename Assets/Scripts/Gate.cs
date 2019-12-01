using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GateType {
    HORIZONTAL,
    VERTICAL
}

public class Gate : MonoBehaviour
{
    [SerializeField] private Animator anim;
    public GateType gateType;
    private bool m_open = true;
    [SerializeField] private SpriteRenderer m_spriteRenderer;
    [SerializeField] private Sprite m_openSpr;
    [SerializeField] private Sprite m_closedSpr;
    [SerializeField] private Vector3 m_horizontalRotation;
    [SerializeField] private Vector3 m_verticalRotation;

    public void UpdateSprite() {
        if (m_open) {
            m_spriteRenderer.sprite = m_openSpr;
        } else {
            m_spriteRenderer.sprite = m_closedSpr;
        }
        if (gateType == GateType.HORIZONTAL) {
            transform.eulerAngles = m_horizontalRotation;
        } else if (gateType == GateType.VERTICAL) {
            transform.eulerAngles = m_verticalRotation;
        }
    }

    public void CloseGate() {
        m_open = false;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Piece") {
            anim.SetTrigger("CloseGate");
        }
    }
}
