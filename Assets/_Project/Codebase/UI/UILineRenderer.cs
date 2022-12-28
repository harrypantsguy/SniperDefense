using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Codebase.UI
{
    public class UILineRenderer : MaskableGraphic
    {
        public float thickness;
        [SerializeField] private bool _localSpace;
        public List<Vector2> points = new List<Vector2>();
        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();

            float width = rectTransform.rect.width;
            float height = rectTransform.rect.height;

            if (points.Count < 2) return;

            Vector2 incomingDir = Vector2.zero;
            for (int i = 0; i < points.Count; i++)
            {
                Vector2 outgoingDir;
                Vector2 vertex = points[i] - (_localSpace ? Vector2.zero : (Vector2)transform.position);
                if (i != points.Count - 1)
                {
                    Vector2 point2 = points[i + 1] - (_localSpace ? Vector2.zero : (Vector2)transform.position);
                    outgoingDir = (point2 - vertex).normalized;
                }
                else
                    outgoingDir = Vector2.zero;
                Vector2 cornerDir = Vector2.Perpendicular(Vector2.Lerp(incomingDir, outgoingDir, .5f)).normalized;
                //Debug.Log($"{incomingDir} {outgoingDir} -> {cornerDir}");
                Vector2 thicknessOffset = cornerDir * thickness / 2f;
                
                CreateVertex(vh, vertex + thicknessOffset);
                CreateVertex(vh, vertex - thicknessOffset);
                incomingDir = outgoingDir;
            } 
            
            int totalVerts = points.Count * 2;
            for (int i = 0; i < totalVerts - 2; i++)
            {
                int vert1 = i;
                int vert2 = i + 1;
                int vert3 = i + 2;
                vh.AddTriangle(vert1, vert2, vert3);
            }
        }

        public void UpdateGraphic()
        {
            SetVerticesDirty();
        }

        private void CreateVertex(VertexHelper vh, Vector2 pos)
        {
            UIVertex vertex = UIVertex.simpleVert;
            vertex.color = color;

            vertex.position = pos;
            vh.AddVert(vertex);
        }
    }
}