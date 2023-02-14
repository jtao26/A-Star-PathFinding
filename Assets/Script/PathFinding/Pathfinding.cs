using System.Collections.Generic;
using UnityEngine;


public class Pathfinding : MonoBehaviour
{

    private const int MOVE_COST = 10;
    private const int MOVE_DIAG_COST = 14;

    private int width, height;
    private Node[,] map;
    private List<Node> waitingList;
    private List<Node> closedList;

    public List<Node> FindPath(Node[,] nodeMap, Node startNode, Node endNode) {
        width = nodeMap.GetLength(0);
        height = nodeMap.GetLength(1);
        map = nodeMap;

        waitingList = new List<Node>() { startNode };
        closedList = new List<Node>();

        for(int i=0; i<width; i++) {
            for(int j=0; j<height; j++) {
                Node n = nodeMap[i, j];
                if (n.accessable) {
                    n.gCost = int.MaxValue;
                    n.hCost = CalcualteDistance(n, endNode);
                    n.CalculateFCost();
                    n.cameFromNode = null;
                }
            }
        }

        startNode.gCost = 0;
        startNode.hCost = CalcualteDistance(startNode, endNode);
        startNode.CalculateFCost();

        while(waitingList.Count > 0 ) {
            Node currentNode = GetLowestFCostNode(waitingList);
            //Debug.Log("Visit Node " + currentNode.ToString());
            if(currentNode.GetX() == endNode.GetX() && currentNode.GetY() == endNode.GetY()) {
                return FormPath(currentNode);
            }

            waitingList.Remove(currentNode);
            closedList.Add(currentNode);

            List<Node> neighbors = GetNodeNeighbors(currentNode);
            for(int i=0; i<neighbors.Count; i++) {
                if (!neighbors[i].accessable || closedList.Contains(neighbors[i]))
                    continue;
                int currGCost = currentNode.gCost + CalcualteDistance(currentNode, neighbors[i]);
                if(currGCost < neighbors[i].gCost) {
                    neighbors[i].cameFromNode = currentNode;
                    neighbors[i].gCost = currGCost;
                    neighbors[i].hCost = CalcualteDistance(neighbors[i], endNode);
                    neighbors[i].CalculateFCost();

                    if (!waitingList.Contains(neighbors[i])) {
                        //Debug.Log("Add Node " + neighbors[i].ToString() + ", came from " + neighbors[i].cameFromNode.ToString());
                        waitingList.Add(neighbors[i]);
                    }
                }
            }
        }

        return null;
    }

    private List<Node> GetNodeNeighbors(Node node) {
        List<Node> neighbors = new List<Node>();
        if(node.GetX() - 1 >= 0) {
            // left
            neighbors.Add(map[node.GetX() - 1, node.GetY()]);
            // btm left
            if (node.GetY() - 1 >= 0)
                neighbors.Add(map[node.GetX() - 1, node.GetY() - 1]);
            // top left
            if (node.GetY() + 1 < height)
                neighbors.Add(map[node.GetX() - 1, node.GetY() + 1]);
        }
        if(node.GetX() + 1 < width) {
            // right
            neighbors.Add(map[node.GetX() + 1, node.GetY()]);
            // btm right
            if (node.GetY() - 1 >= 0)
                neighbors.Add(map[node.GetX() + 1, node.GetY() - 1]);
            // top right
            if (node.GetY() + 1 < height)
                neighbors.Add(map[node.GetX() + 1, node.GetY() + 1]);
        }
        // btm
        if (node.GetY() - 1 >= 0)
            neighbors.Add(map[node.GetX(), node.GetY() - 1]);
        // top
        if (node.GetY() + 1 < height)
            neighbors.Add(map[node.GetX(), node.GetY() + 1]);
        
        return neighbors;
    }

    private int CalcualteDistance(Node n1, Node n2) {
        int deltaX = Mathf.Abs(n1.GetX() - n2.GetX());
        int deltaY = Mathf.Abs(n1.GetY() - n2.GetY());
        int diagDistance = Mathf.Min(deltaY, deltaX);
        return diagDistance * MOVE_DIAG_COST + Mathf.Abs(deltaX - deltaY) * MOVE_COST;
    }

    private Node GetLowestFCostNode(List<Node> nodeList) {
        Node lowest = nodeList[0];
        for(int i=0; i < nodeList.Count; i++) {
            //Debug.Log("Node " + nodeList[i].ToString() + " fcost " + nodeList[i].fCost + " gcost" + nodeList[i].gCost + ", minimum fcost " + lowest.ToString() + ", " + lowest.fCost);
            if (nodeList[i].fCost < lowest.fCost) {
                lowest = nodeList[i];
            }
        }
        return lowest;
    }

    private List<Node> FormPath(Node endNode) {
        List<Node> path = new List<Node>();
        path.Add(endNode);
        Node currentNode = endNode;
        while(currentNode.cameFromNode != null) {
            path.Add(currentNode.cameFromNode);
            currentNode = currentNode.cameFromNode;
        }
        path.Reverse();
        return path;
    }
}
