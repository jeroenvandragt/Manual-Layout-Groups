using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace EasySee.UI
{
    [AddComponentMenu( "Layout/EasySee/ManualHorizontalLayout" )]
	[ExecuteInEditMode]
    public class LayoutHorizontal : MonoBehaviour
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

        RectTransform.Edge edgeToSnap = RectTransform.Edge.Left;


        void Start ( )
        {
            m_Transform = GetComponent<RectTransform>( );
            GetRectAndCalculate( );
        }
        /// <summary>
        /// Callback for when the amount of children has changed
        /// </summary>
        private void OnTransformChildrenChanged ( )
        {
            if (updateOnChildChange)
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
            Debug.Log( "GetRectAndCalculate" );
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
                Debug.Log( "Parent transform is null." );
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
            if (children == null || children.Count <= 0)
            {
                Debug.Log( "No Children to calculate position for." );
                return;
            }
            float offset = 0;
            foreach (RectTransform child in children)
            {
                if (child.gameObject.activeSelf)
                {
                    child.anchorMin = new Vector2( 0, 0 );
                    child.anchorMax = new Vector2( 0, 0 );

                    child.sizeDelta = new Vector2( m_Transform.rect.width, child.rect.height );
                    child.SetInsetAndSizeFromParentEdge( edgeToSnap, offset, childSize.x );
                    child.anchoredPosition = new Vector2( child.anchoredPosition.x, m_Transform.rect.height / 2 );
                    child.SetSizeWithCurrentAnchors( RectTransform.Axis.Vertical, childSize.y );
                    offset += childSize.x + padding;
                }
            }
            if (adjustSizeToContent)
            {
                m_Transform.sizeDelta = new Vector2( offset, m_Transform.sizeDelta.y );
            }
        }
    }
}