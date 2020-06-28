using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//control all puzzles movement, checking and activating
public class PuzzleManager : MonoBehaviour
{
    private Dictionary<string, List<PuzzlePiece>> pieces;
    private Dictionary<string, List<PuzzleMove>> move;

    private void Start()
    {
        pieces = new Dictionary<string, List<PuzzlePiece>>();
        move = new Dictionary<string, List<PuzzleMove>>();

        //get all pieces sort into groups
        PuzzlePiece[] piecers = FindObjectsOfType<PuzzlePiece>();
        for (int i = 0; i < piecers.Length; i++)
        {
            if(!pieces.ContainsKey(piecers[i].GetGroup()))
            {
                List<PuzzlePiece> list = new List<PuzzlePiece>();
                list.Add(piecers[i]);
                pieces.Add(piecers[i].GetGroup(), list);
            }
            else
            {
                pieces[piecers[i].GetGroup()].Add(piecers[i]);
            }
        }

        //get all moves sort into groups
        PuzzleMove[] movers = FindObjectsOfType<PuzzleMove>();
        for (int i = 0; i < movers.Length; i++)
        {
            if (!move.ContainsKey(movers[i].GetGroup()))
            {
                List<PuzzleMove> list = new List<PuzzleMove>();
                list.Add(movers[i]);
                move.Add(movers[i].GetGroup(), list);
            }
            else
            {
                move[movers[i].GetGroup()].Add(movers[i]);
            }
        }
    }

    //check group passed in
    private void CheckPuzzle(string groupName)
    {
        //if all pieces true
        foreach(PuzzlePiece p in pieces[groupName])
        {
            if(!p.GetIsActive())
            {
                //something in group off reset moves for group
                ResetPuzzle(groupName);
                return;
            }
        }

        //all true
        MovePuzzle(groupName);
    }

    private void MovePuzzle(string groupName)
    {
        //move all movealbe pieces in group
        foreach(PuzzleMove m in move[groupName])
        {
            //activate the move function
            m.NextPosition();
        }
    }

    private void ResetPuzzle(string groupName)
    {
        //reset all movealbe pieces in group
        foreach (PuzzleMove m in move[groupName])
        {
            //reset 
            m.ResetPosition();
        }
    }

    private void OnEnable()
    {
        PuzzlePiece.activated += CheckPuzzle;
    }
}
