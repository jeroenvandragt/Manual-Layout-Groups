using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace EasySee.UI
{
    [AddComponentMenu("Layout/EasySee/ManualVerticalLayout")]
	[ExecuteInEditMode]
    public class LayoutVertical : MonoBehaviour
    {
         List<RectTransform> children;
        RectTransform m_Transform;

        [SerializeField]
        Vector2 childSize = new Vector2( );

        [SerializeField]
        float padding = 0;

        [SerializeField]
        bool adjustSizeToContent = false;

        public bool updateOnChildChange = true;


        RectTransform.Edge edgeToSnap = RectTransform.Edge.Top;

        public void Start ( )
        {
            m_Transform = GetComponent<RectTransform>( );
            GetRectAndCalculate( );
        }
        /// <summary>
        /// Callback for when the amount of children has changed
        /// </summary>
        private void OnTransformChildrenChanged ( )
        {
            if(updateOnChildChange)
            {
                GetChildrenRects( );
                Invoke( "CalculatePosition", 0.1f );
            }
        }

        /// <summary>
        /// Called to get all the children in the list and refresh the position of the children.
        /// </summary>
        public void GetRectAndCalculate ( )
        {
            GetChildrenRects( );
            CalculatePosition( );
        }

        /// <summary>
        /// Called to collect all the children in the list.
        /// </summary>
        public void GetChildrenRects ( )
        {
            if (children == null)
            {
                children = new List<RectTransform>( );
            }
            if (children.Count > 0)
            {
                children.Clear( );
            }
            if (m_Transform == null)
            {
                m_Transform = GetComponent<RectTransform>( );
                Debug.Log( "Parent transform is null. Did you try to add this to a non-UI object?", gameObject.transform );
            }
            else
            {
                for (int i = 0; i < m_Transform.childCount; i++)
                {
                    children.Add( m_Transform.GetChild( i ).GetComponent<RectTransform>( ) );
                }
            }
        }

        /// <summary>
        /// This is called when we need to refresh the positioning of the children.
        /// </summary>
        public void CalculatePosition ( )
        {
            if(!m_Transform)
                return;

            if (children == null || children.Count <= 0)
            {
                Debug.Log( "No Children to calculate position for." );
                return;
            }
            float offset = 0;
            foreach (RectTransform child in children.ToArray())
            {
                if (child.gameObject.activeSelf)
                {
                    child.anchorMin = new Vector2( 0, 1 );
                    child.anchorMax = new Vector2( 0, 1 );

                    child.sizeDelta = new Vector2( m_Transform.rect.width, child.rect.height );
                    
                    child.SetInsetAndSizeFromParentEdge( edgeToSnap, offset, childSize.y );
                    child.anchoredPosition = new Vector2(m_Transform.rect.width/2, child.anchoredPosition.y );
                    child.SetSizeWithCurrentAnchors( RectTransform.Axis.Horizontal, childSize.x );
                    offset += childSize.y + padding;
                }
            }
            if (adjustSizeToContent)
            {
                m_Transform.sizeDelta = new Vector2( m_Transform.sizeDelta.x, offset );
            }
        }
    }
}
