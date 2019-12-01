using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileType {
    NONE,
    FLOOR,
    WALL,
    GATE_OPEN,
    GATE_CLOSED,

    GOAL_1,
    GOAL_2,
    GOAL_3,
    GOAL_4
}

public enum EntityType {
    NONE,
    
    OBJ_1,
    OBJ_2,
    OBJ_3,
    OBJ_4
}

public enum Direction {
    UP,
    DOWN,
    LEFT,
    RIGHT
}

public class Tile
{
    public TileType tileType;
    public EntityType entityType;
    public Coord coord;

    public bool IsTraversible() {
        return entityType == EntityType.NONE && (tileType == TileType.NONE || tileType == TileType.FLOOR || tileType == TileType.GATE_OPEN || IsGoal(tileType));
    }

    public static bool IsGoal(TileType tileType) {
        return tileType == TileType.GOAL_1 || tileType == TileType.GOAL_2 || tileType == TileType.GOAL_3 || tileType == TileType.GOAL_4;
    }

    // Returns the direction vector associated with the given Direction
    public static Vector3 GetDirectionVector(Direction dir) {
        if (dir == Direction.UP) {
            return Vector3.up;
        } else if (dir == Direction.DOWN) {
            return Vector3.down;
        } else if (dir == Direction.LEFT) {
            return Vector3.left;
        } else if (dir == Direction.RIGHT) {
            return Vector3.right;
        }
        return Vector3.zero;
    }
}
