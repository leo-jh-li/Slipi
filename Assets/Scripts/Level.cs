using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level
{
    public Tile[,] tiles;

    public float tileDist;
    
    private Dictionary<Coord, List<GameObject>> m_levelObjects = new Dictionary<Coord, List<GameObject>>();
    public List<Piece> pieces = new List<Piece>();

    // Add given GameObject to the m_levelObjects dict, creating a new List if necessary
    public void AddLevelObject(Coord key, GameObject value) {
        if (m_levelObjects.ContainsKey(key)) {
            m_levelObjects[key].Add(value);
        } else {
            m_levelObjects[key] = new List<GameObject>() { value };
        }
    }

    public Vector3 CoordToPos(Coord coord) {
        Vector2 centreOffset = new Vector2(tiles.GetLength(1) / 2 * tileDist / 2, tiles.GetLength(0) / 2 * tileDist / 2);
        return new Vector3(coord.x * tileDist / 2 - centreOffset.x, -coord.y * tileDist / 2 + centreOffset.y, 0);
    }

    public bool ValidCoord(Coord coord) {
        return coord.x >= 0 && coord.x < tiles.GetLength(1) && coord.y >= 0 && coord.y < tiles.GetLength(0);
    }

    public Tile GetTile(Coord coord) {
        return tiles[coord.y, coord.x];
    }

    public int GetWidth() {
        return tiles.GetLength(1) / 2;
    }

    // Moves given Piece moveDistance tiles in given direction if possible and returns true iff the complete movement was successful.
    public bool MovePiece(Piece piece, Direction dir, int moveDistance, int turnNum) {
        Coord startCoord = piece.coord;
        Coord destCoord = piece.coord.ApplyMovement(dir, moveDistance);
        if (!ValidCoord(startCoord)) {
            return false;
        }
        if (startCoord.x != destCoord.x && startCoord.y != destCoord.y) {
            return false;
        }
        Coord currCoord = startCoord;
        do {
            Coord gateCoord = null;
            // Check for gates
            currCoord = currCoord.ApplyMovement(dir, 1);
            if (!ValidCoord(currCoord)) {
                return false;
            }
            Tile currTile = GetTile(currCoord);
            if (!currTile.IsTraversible()) {
                return false;
            } else if (currTile.tileType == TileType.GATE_OPEN) {
                // Mark gate to be closed if movement is successful
                gateCoord = currCoord;
            }
            // Check next tile
            currCoord = currCoord.ApplyMovement(dir, 1);
            currTile = GetTile(currCoord);
            if (!currTile.IsTraversible()) {
                return false;
            }

            // This iteration's movement was successful, so update the previous and new tiles' entity info
            GetTile(piece.coord).entityType = EntityType.NONE;
            GetTile(currCoord).entityType = piece.entityType;
            piece.coord = currCoord;

            // Close gate if one was passed
            if (gateCoord != null) {
                GetTile(gateCoord).tileType = TileType.GATE_CLOSED;
                foreach (GameObject obj in m_levelObjects[gateCoord]) {
                    Gate gate = obj.GetComponent<Gate>();
                    if (gate != null) {
                        gate.CloseGate(turnNum);
                        break;
                    }
                }
            }

            // Check for goals
            if (Tile.IsGoal(currTile.tileType)) {
                if (piece != null && piece.goalType == currTile.tileType) {
                    piece.placedInGoal = true;
                    currTile.entityType = EntityType.NONE;
                    break;
                }
                // No more need to move this Piece
                return true;
            }
        } while (currCoord != destCoord && ValidCoord(currCoord));
        return true;
    }

    // Return true if any Pieces are currently in a slide animation
    public bool PiecesInMotion() {
        foreach (Piece piece in pieces) {
            if (piece.inMotion) {
                return true;
            }
        }
        return false;
    }

    // Attempts to move all Pieces in given direction. Returns true if any movement was successful.
    public bool MoveAllPieces(Direction dir) {
        bool anyMoveMade = false;
        bool piecesMovedThisTurn = true;
        if (!PiecesInMotion()) {
            int turnNum = 1;
            // Repeat until no pieces no longer move
            while (piecesMovedThisTurn) {
                piecesMovedThisTurn = false;
                foreach (Piece piece in pieces) {
                    if (!piece.placedInGoal && MovePiece(piece, dir, 2, turnNum)) {
                        piecesMovedThisTurn = true;
                        anyMoveMade = true;
                    }
                }
                turnNum++;
            }
            foreach (Piece piece in pieces) {
                piece.SlideTo(dir, CoordToPos(piece.coord));
            }
        }
        return anyMoveMade;
    }

    // Returns true iff all pieces are in their goals
    public bool PiecesPlacedInGoal() {
        foreach (Piece piece in pieces) {
            if (!piece.placedInGoal || piece.inMotion) {
                return false;
            }
        }
        return true;
    }
}
