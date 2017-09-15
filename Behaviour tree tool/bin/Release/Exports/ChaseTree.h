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
          TreeNode * n12 = new TreeNode();
          m_nodes.push_back(n12);
          
          m_startNode = n12;
          
          TreeNode * n13 = new TreeNode();
          m_nodes.push_back(n13);
          
          TreeNode * n14 = new TreeNode();
          m_nodes.push_back(n14);
          
          TreeNode * n15 = new TreeNode();
          m_nodes.push_back(n15);
          
          TreeNode * n16 = new TreeNode();
          m_nodes.push_back(n16);
          
          TreeNode * n17 = new TreeNode();
          m_nodes.push_back(n17);
          
          TreeNode * n18 = new TreeNode();
          m_nodes.push_back(n18);
          
          //////////////////////////////////////////////////////
          //////////////////////////////////////////////////////
      
          //chase
          n12->run = [n12]()->bool
          {
             
             //runs through all its children Nodes and returns true if they all return true, used to preform and logic gates
             for each(TreeNode * n in n12->m_children)
             {
                 if(n->run() == false)
                 {
                     return false;
                 }
             }
             
             return false;
          };
          
          //check enemy close
          n13->run = [n13]()->bool
          {
             
             //runs through all its children Nodes and returns true if they all return true, used to preform and logic gates
             for each(TreeNode * n in n13->m_children)
             {
                 if(n->run() == false)
                 {
                     return false;
                 }
             }
             
             return false;
          };
          
          //Default Description
          n14->run = [n14]()->bool
          {
             
             //runs through all its children Nodes and returns true if they all return true, used to preform and logic gates
             for each(TreeNode * n in n14->m_children)
             {
                 if(n->run() == false)
                 {
                     return false;
                 }
             }
             
             return false;
          };
          
          //move to enemy
          n15->run = [n15]()->bool
          {
          
             //code for desiered node action here
             
             return true;
          };
          
          //attack enemy
          n16->run = [n16]()->bool
          {
          
             //code for desiered node action here
             
             return true;
          };
          
          //check if can see enemy
          n17->run = [n17]()->bool
          {
             
             //code for desiered node check here
             if(true)
             {
                 return true;
             }
             
             return false;
          };
          
          //enemy within attack range
          n18->run = [n18]()->bool
          {
             
             //code for desiered node check here
             if(true)
             {
                 return true;
             }
             
             return false;
          };
          
          //////////////////////////////////////////////////////
          //////////////////////////////////////////////////////
      
          n12->m_children.push_back(n13);
          n12->m_children.push_back(n14);
          
          n13->m_children.push_back(n17);
          n13->m_children.push_back(n15);
          
          n14->m_children.push_back(n18);
          n14->m_children.push_back(n16);
          
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
