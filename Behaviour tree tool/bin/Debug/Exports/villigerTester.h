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
          
          TreeNode * n4 = new TreeNode();
          m_nodes.push_back(n4);
          
          TreeNode * n5 = new TreeNode();
          m_nodes.push_back(n5);
          
          TreeNode * n9 = new TreeNode();
          m_nodes.push_back(n9);
          
          TreeNode * n15 = new TreeNode();
          m_nodes.push_back(n15);
          
          TreeNode * n16 = new TreeNode();
          m_nodes.push_back(n16);
          
          //////////////////////////////////////////////////////
          //////////////////////////////////////////////////////
      
          //Do Other thing!
          n0->run = [n0]()->bool
          {
          
             //code for desiered node action here
             
             return true;
          };
          
          //Do thing!
          n1->run = [n1]()->bool
          {
          
             //code for desiered node action here
             
             return true;
          };
          
          //thing?
          n2->run = [n2]()->bool
          {
             
             //code for desiered node check here
             if(true)
             {
                 return true;
             }
             
             return false;
          };
          
          //Testlingz
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
          
          //Start Node
          n4->run = [n4]()->bool
          {
             
             //runs through all its children Nodes and returns true if they all return true, used to preform and logic gates
             for each(TreeNode * n in n4->m_children)
             {
                 if(n->run() == false)
                 {
                     return false;
                 }
             }
             
             return false;
          };
          
          //bad logic
          n5->run = [n5]()->bool
          {
             int i = rand() % n5->m_children.size();
             int count = 0;
             
             //runs through its children Nodes assuming the delegator value check is true
             for each(TreeNode * n in n5->m_children)
             {
                 if(count == i)
                 {
                     return n->run();
                 }
                 count++;
             }
             
              return true;
          };
          
          //A Bad Node
          n9->run = [n9]()->bool
          {
          
             //code for desiered node action here
             
             return true;
          };
          
          //Default Description
          n15->run = [n15]()->bool
          {
             
             //runs through its children Nodes assuming the delegator value check is true
             if(true)
             {
                 for each(TreeNode * n in n15->m_children)
                 {
                     n->run();
                 }
             }
             
             //Implament your own true false logic, this is just a holder
             return true;
          };
          
          //Default Description
          n16->run = [n16]()->bool
          {
          
             //code for desiered node action here
             
             return true;
          };
          
          //////////////////////////////////////////////////////
          //////////////////////////////////////////////////////
      
          n3->m_children.push_back(n0);
          n3->m_children.push_back(n1);
          n3->m_children.push_back(n2);
          
          n4->m_children.push_back(n3);
          n4->m_children.push_back(n5);
          n4->m_children.push_back(n15);
          
          n5->m_children.push_back(n9);
          
          n15->m_children.push_back(n16);
          
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
