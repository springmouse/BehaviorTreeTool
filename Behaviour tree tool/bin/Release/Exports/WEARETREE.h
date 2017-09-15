#pragma once
#include <list>
#include <functional>

/*
* class: BehaviourTree
* --------------------
*
* ____________________
*
* Author: ____________
*/
class BehaviourTree
{
public:
      /*
      * class: TreeNode
      * ---------------
      *
      * _______________
      *
      * Author: _______
      */
      class TreeNode
      {
      public:
          //the list of the nodes children
          std::list<TreeNode *> m_children;
          
          //the delegate function we will assign to exacute
          std::function<bool()> run;
      };

      //List of all the nodes making up this tree
      std::list<TreeNode *> m_nodes;
      
      //the node at the begining of the tree
      TreeNode * m_startNode = nullptr;

      /*
      * Function: Constructor
      * ---------------------
      *
      * Default constructor
      *
      */
      BehaviourTree()
      {
          SetUpNodes();
      };

      /*
      * Function: DeConstructor
      * ---------------------
      *
      * Default DeConstructor
      *
      */
      ~BehaviourTree()
      {
          m_nodes.clear();
      };

      /*
      * Function: SetUpNodes
      * ---------------------
      *
      * this function sets up all the nodes in the tree
      * assiging them there functions and linking them up
      *
      * Parameters: none
      *
      *returns: returns nothing as it is a void function
      */
      void SetUpNodes()
      {
          TreeNode * n0 = new TreeNode();
          m_nodes.push_back(n0);
          
          m_startNode = n0;
          
          TreeNode * n1 = new TreeNode();
          m_nodes.push_back(n1);
          
          TreeNode * n2 = new TreeNode();
          m_nodes.push_back(n2);
          
          TreeNode * n3 = new TreeNode();
          m_nodes.push_back(n3);
          
          //////////////////////////////////////////////////////
          //////////////////////////////////////////////////////
      
          //Default Description
          n0->run = [n0]()->bool
          {
          
             //code for desiered node action here
             
             return true;
          };
          
          //Default Description
          n1->run = [n1]()->bool
          {
          
             //code for desiered node action here
             
             return true;
          };
          
          //Default Description
          n2->run = [n2]()->bool
          {
          
             //code for desiered node action here
             
             return true;
          };
          
          //Default Description
          n3->run = [n3]()->bool
          {
             
             //runs through its children Nodes and returns true for the first child that returns true, used to reform or logic gates
             for each(TreeNode * n in n3->m_children)
             {
                 if(n->run() == true)
                 {
                     return true;
                 }
             }
             
             return true;
          };
          
          //////////////////////////////////////////////////////
          //////////////////////////////////////////////////////
      
          n3->m_children.push_back(n0);
          n3->m_children.push_back(n1);
          n3->m_children.push_back(n2);
          
      };

      /*
      * Function: Update
      * ----------------
      *
      * this function runs the behaviour tree each time it is called
      *
      * Parameters: none
      *
      *returns: returns nothing as it is a void function
      */
      void Update()
      {
          
          m_startNode->run();
          
      };
};
