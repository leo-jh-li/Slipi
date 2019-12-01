using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    public Coord coord;
    [HideInInspector] public EntityType entityType;
    [HideInInspector] public bool inMotion;
    public TileType goalType;       // The goal type for this Piece
    public bool placedInGoal;
    [SerializeField] private string m_finishedPieceLayerName;
    private float m_slideSpeed;
    private static System.Func<bool> m_onPieceStop;

    private void Start() {
        m_slideSpeed = Constants.instance.PIECE_SLIDE_SPEED;
    }

    public void SlideTo(Direction dir, Vector3 destination) {
        if (destination != transform.localPosition) {
            AudioManager.instance.Play(Constants.instance.SLIDE_SFX_NAME);
            StartCoroutine(AnimateSlide(dir, destination));
        }
    }

    private IEnumerator AnimateSlide(Direction dir, Vector3 destination) {
        inMotion = true;
        Vector3 incVect = Tile.GetDirectionVector(dir);
        do {
            transform.localPosition += incVect * Time.deltaTime * m_slideSpeed;
            // Clamp if destination was passed
            if (dir == Direction.LEFT && transform.localPosition.x <= destination.x ||
                dir == Direction.RIGHT && transform.localPosition.x >= destination.x || 
                dir == Direction.UP && transform.localPosition.y >= destination.y || 
                dir == Direction.DOWN && transform.localPosition.y <= destination.y) {
                transform.localPosition = destination;
            }
            yield return null;
        } while (transform.localPosition != destination);
        if (placedInGoal) {
            GetComponent<SpriteRenderer>().sortingLayerName = m_finishedPieceLayerName;
        }
        inMotion = false;

        // Play one of three sound effects
        if (m_onPieceStop()) {
            AudioManager.instance.Play(Constants.instance.LEVEL_COMPLETE_SFX_NAME);
        } else if (placedInGoal) {
            AudioManager.instance.Play(Constants.instance.GOAL_SFX_NAME);
        } else {
            AudioManager.instance.Play(Constants.instance.COLLISION_SFX_NAME);
        }
    }

    public static void SetVictoryCheckCallback(System.Func<bool> callback) {
        m_onPieceStop = callback;
    }

    public void PlaceInGoal() {
        placedInGoal = true;
    }
}
