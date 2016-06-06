using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
////////////////////////////////
using System.Windows.Automation;
using System.Windows;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security;
using System.Threading;
using System.Windows.Forms;

namespace computer_assisted_instruction
{
    public class UITree
    {
        #region DllImport
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern IntPtr FindWindow(
          string lpClassName, string lpWindowName);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern bool AttachThreadInput(
            int idAttach, int idAttachTo, bool fAttach);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern int GetWindowThreadProcessId(
            IntPtr hWnd, IntPtr lpdwProcessId);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern IntPtr SetForegroundWindow(IntPtr hWnd);
        #endregion DllImport
        public AutomationElement GetElementFromPoint(Point pt)
        {
            return AutomationElement.FromPoint(pt);
        }
        public TreeNode GetParentsFromElemnt(AutomationElement rootElement,int level)
        {
            TreeNode tn = WalkUpRawElements(rootElement, level);
            return tn;
        }
        public TreeNode GetChildsFromElement(AutomationElement rootElement,int level)
        {
            TreeNode tn = new TreeNode("Root:"+rootElement.Current.Name);
            WalkDownRawElements(rootElement, tn, level);
            return tn;
        }
        public bool SetForegroundWindow(AutomationElement elm, uint retries)
        {
            //Util.ValidateNotNull("elm", elm);

            try
            {
                if (retries < 5)
                {
                    // Using Win32 to set foreground window because
                    // AutomationElement.SetFocus() is unreliable

                    // Get handle to the element
                    IntPtr other = FindWindow(null, elm.Current.Name);

                    // Get the Process ID for the element we are trying to
                    // set as the foreground element
                    int other_id = GetWindowThreadProcessId(
                        other, IntPtr.Zero);

                    // Get the Process ID for the current process
                    int this_id = GetWindowThreadProcessId(
                        Process.GetCurrentProcess().Handle, IntPtr.Zero);

                    // Attach the current process's input to that of the 
                    // given element. We have to do this otherwise the
                    // WM_SETFOCUS message will be ignored by the element.
                    bool success =
                        AttachThreadInput(this_id, other_id, true);

                    // Make the Win32 call
                    IntPtr previous = SetForegroundWindow(other);

                    if (IntPtr.Zero.Equals(previous))
                    {
                        // Trigger re-try
                        throw new Exception(
                            "SetForegroundWindow failed");
                    }
                    else
                    {
                        //Log("focus set");
                    }

                    return true;
                }

                // Exceeded retry limit, failed!
                return false;
            }
            catch
            {
                retries++;
                //uint time = retries * RETRY_FACTOR;
                //Log("  Could not SetFocus(), retry in {0} ms", time);
                Thread.Sleep(100);
                return SetForegroundWindow(elm, retries);
            }
        }
        public bool SetFocus(AutomationElement elm)
        {
            elm.SetFocus();
            return true;
        }
        private void WalkDownRawElements(AutomationElement rootElement, TreeNode treeNode,int level)
        {
            // Conditions for the basic views of the subtree (content, control, and raw)  
            // are available as fields of TreeWalker, and one of these is used in the  
            // following code.
            AutomationElement elementNode = TreeWalker.RawViewWalker.GetFirstChild(rootElement);
            //AutomationElement elementNode = TreeWalker.ControlViewWalker.GetFirstChild(rootElement);
            while (elementNode != null)
            {
                TreeNode childTreeNode = treeNode.Nodes.Add( 
                    elementNode.GetRuntimeId()[0].ToString() + "##" +
                    elementNode.Current.FrameworkId + "##" +
                    elementNode.Current.AutomationId + "##" +
                    elementNode.Current.ControlType.LocalizedControlType + "##" +
                    elementNode.Current.Name);
                if (level > 0)
                    WalkDownRawElements(elementNode, childTreeNode,level-1);
                elementNode = TreeWalker.RawViewWalker.GetNextSibling(elementNode);
                //elementNode = TreeWalker.ControlViewWalker.GetNextSibling(elementNode);
            }
        }
        public TreeNode WalkUpRawElements(AutomationElement rootElement, int level)
        {
            TreeNode treeNode;
            Stack<TreeNode> stack = new Stack<TreeNode>();
            AutomationElement elementNode = rootElement;
            do
            {
                TreeNode childTreeNode = new TreeNode(
                    //elementNode.GetRuntimeId()[0].ToString() + " ## " +
                    //elementNode.Current.FrameworkId + " ## " +
                    elementNode.Current.AutomationId + "##" +
                    //elementNode.Current.ProcessId + "##" +
                    elementNode.Current.ControlType.LocalizedControlType + "##" +
                    elementNode.Current.Name
                    );
                stack.Push((TreeNode)childTreeNode);

                elementNode = TreeWalker.RawViewWalker.GetParent(elementNode);
            } while (elementNode != null && level-- > 0);

            treeNode = (TreeNode)stack.Pop().Clone();
            TreeNode rootNode = treeNode;
            while (stack.Count() > 0)
            {
                TreeNode childTreeNode = stack.Pop();
                rootNode.Nodes.Add(childTreeNode);
                rootNode = childTreeNode;
            }
            return treeNode;
        }
    }
}
//if (elementNode.Current.Name.Contains("Studio"))
//{
//SetForegroundWindow(elementNode, 0);
//elementNode.SetFocus();
//System.Windows.Point p;
//if(elementNode.TryGetClickablePoint(out p))
//{
//    System.Drawing.Point cp = new System.Drawing.Point((int)p.X,(int) p.Y);
//    Cursor.Position = cp;
//}
//break;
//}
////////////////////////////////////////////////////
//Stack<TreeNode> stack = new Stack<TreeNode>();
//            AutomationElement elementNode = rootElement;
//            do
//            {
//                TreeNode childTreeNode = new TreeNode(
//                    //elementNode.GetRuntimeId()[0].ToString() + " ## " +
//                    //elementNode.Current.FrameworkId + " ## " +
//                    elementNode.Current.AutomationId + " ## " +
//                    elementNode.Current.ControlType.LocalizedControlType + " ## " +
//                    elementNode.Current.Name
//                    );
//                stack.Push(childTreeNode);

//                elementNode = TreeWalker.RawViewWalker.GetParent(rootElement);
//            } while (elementNode != null && level-- > 0);
            
//            TreeNode rootNode = stack.Pop();
//            treeNode = rootNode;
//            while(stack.Count() > 0)
//            {
//                TreeNode childTreeNode  = stack.Pop();
//                rootNode.Nodes.Add(childTreeNode);
//                rootNode = childTreeNode;
//            }