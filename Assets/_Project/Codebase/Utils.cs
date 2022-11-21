using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Project.Codebase
{
    public static class Utils
    {
        private static Camera _mainCam;

        public static float GetFloatListSum(List<float> list)
        {
            float sum = 0f;
            foreach (float val in list)
                sum += val;
            return sum;
        }

        public static int GetIntListSum(List<int> list)
        {
            int sum = 0;
            foreach (int val in list)
                sum += val;
            return sum;
        }

        public static Vector3 Abs(this Vector3 vec)
        {
            return new Vector3(Mathf.Abs(vec.x), Mathf.Abs(vec.y), Mathf.Abs(vec.z));
        }

        public static Vector2 Abs(this Vector2 vec)
        {
            return new Vector2(Mathf.Abs(vec.x), Mathf.Abs(vec.y));
        }
        
        public static bool TryRaycastAlongLine(Vector3 from, Vector3 to, out RaycastHit hit, LayerMask layerMask)
        {
            Vector3 dir = to - from;
            float dist = dir.magnitude;
            dir.Normalize();

            return Physics.Raycast(from, dir, out hit, dist, layerMask);
        }
        
        public static bool TryRaycastAlongLine(Vector3 from, Vector3 to, out RaycastHit hit)
        {
            Vector3 dir = to - from;
            float dist = dir.magnitude;
            dir.Normalize();

            return Physics.Raycast(from, dir, out hit, dist);
        }
        
        public static bool TrySpherecastAlongLine(Vector3 from, Vector3 to, float radius, 
            out RaycastHit hit, LayerMask layerMask)
        {
            Vector3 dir = to - from;
            float dist = dir.magnitude;
            dir.Normalize();

            return Physics.SphereCast(from, radius, dir, out hit, dist, layerMask);
        }
        
        public static bool TrySpherecastAlongLine(Vector3 from, Vector3 to, float radius, out RaycastHit hit)
        {
            Vector3 dir = to - from;
            float dist = dir.magnitude;
            dir.Normalize();

            return Physics.SphereCast(from, radius, dir, out hit, dist);
        }
        
        public static bool TryGetClosestElementInCollection<T>(in Vector3 position, List<T> collection, 
            out T closestElement, float radius = Mathf.Infinity) where T : MonoBehaviour
        {
            closestElement = default;
            
            if (collection.Count == 0) return false;
            
            float closestDist = Mathf.Infinity;
            
            foreach (T element in collection)
            {
                if (element == null) continue;
                if (element.transform == null) continue;

                float dist = Vector3.Distance(position, element.transform.position);

                if (dist > radius) continue;
                
                if (dist < closestDist)
                {
                    closestDist = dist;
                    closestElement = element;
                }
            }

            return closestElement != null;
        }
        
        public static Vector2 WorldMousePos
        {
            get
            {
                return ScreenToWorldPoint(Input.mousePosition);
            }
        }

        public static Vector2 ScreenToWorldPoint(Vector2 screenPos)
        {
            if (_mainCam == null)
                _mainCam = Camera.main;

            if (_mainCam == null)
                return Vector2.zero;
            return _mainCam.ScreenToWorldPoint(screenPos);
        }
        
        public static Vector2 WorldtoScreenPoint(Vector2 worldPos)
        {
            if (_mainCam == null)
                _mainCam = Camera.main;

            return _mainCam.WorldToScreenPoint(worldPos);
        }

        public static Vector2 ProjectOnNormal(Vector2 vector, Vector2 normal)
        {
            return Vector3.ProjectOnPlane(vector, normal);
        }
        
        public static Vector3 RepairHitSurfaceNormal(RaycastHit hit, int layerMask)
        {
            if(hit.collider is MeshCollider collider)
            {
                Mesh mesh = collider.sharedMesh;
                int[] tris = mesh.triangles;
                Vector3[] verts = mesh.vertices;
 
                Vector3 v0 = verts[tris[hit.triangleIndex * 3]];
                Vector3 v1 = verts[tris[hit.triangleIndex * 3 + 1]];
                Vector3 v2 = verts[tris[hit.triangleIndex * 3 + 2]];
 
                Vector3 n = Vector3.Cross(v1 - v0, v2 - v1).normalized;
 
                return hit.transform.TransformDirection(n);
            }

            Vector3 p = hit.point + hit.normal * 0.01f;
            Physics.Raycast(p, -hit.normal, out hit, 0.011f, layerMask);
            return hit.normal;
        }

        public static float RoundToNumber(float value, float number)
        {
            return Mathf.RoundToInt(value / number) * number;
        }

        public static Vector3 DirectionToEuler(Vector3 direction)
        {
            Quaternion rot = Quaternion.LookRotation(direction);
            return rot.eulerAngles;
        }

        public static Vector3 EulerToDirection(Vector3 euler)
        {
            return Quaternion.Euler(euler) * Vector3.forward;
        }

        public static Vector3Int ToVec3Int(this Vector3 vec, bool round = true)
        {
            if (!round)
                return new Vector3Int(
                    (int)vec.x,
                    (int)vec.y,
                    (int)vec.z);
            return new Vector3Int(
                Mathf.RoundToInt(vec.x), 
                Mathf.RoundToInt(vec.y), 
                Mathf.RoundToInt(vec.z));
        }

        public static byte ByteClamp(byte value, byte min, byte max)
        {
            if (value < min)
                return min;
            return value > max ? max : value;
        }

        public static float Remap(this float from, float fromMin, float fromMax, float toMin, float toMax)
        {
            float fromAbs = from - fromMin;
            float fromMaxAbs = fromMax - fromMin;

            float normal = fromAbs / fromMaxAbs;

            float toMaxAbs = toMax - toMin;
            float toAbs = toMaxAbs * normal;

            float to = toAbs + toMin;

            return to;
        }

        public static float Remap01(this float from, float fromMin, float fromMax)
        {
            return Remap(from, fromMin, fromMax, 0f, 1f);
        }

        public static float ClampedRemap(this float from, float fromMin, float fromMax, float toMin, float toMax)
        {
            return Remap(Mathf.Clamp(from, fromMin, fromMax), fromMin, fromMax, toMin, toMax);
        }

        public static Vector3 ToVector3(this Color col)
        {
            return new Vector3(col.r, col.g, col.b);
        }

        public static Color ToColor(this Vector3 vec)
        {
            return new Color(vec.x, vec.y, vec.z);
        }

        public static string UppercaseFirst(this string s)
        {
            if (string.IsNullOrEmpty(s)) return string.Empty;
            return char.ToUpper(s[0]) + s.Substring(1);
        }

        public static T GetRandomElementOfList<T>(this List<T> list)
        {
            return list[Random.Range(0, list.Count)];
        }

        public static bool IsCharAVowel(this char c)
        {
            return "aeiouAEIOU".IndexOf(c) >= 0;
        }

        public static T GetRandomEnum<T>(params int[] excludedIndices) where T : struct, IConvertible
        {
            Type enumType = typeof(T);
            if (!enumType.IsEnum)
                throw new ArgumentException("Not an enum");

            var enumVals = Enum.GetValues(enumType).Cast<int>().ToList();

            enumVals.RemoveAll(excludedIndices.Contains);

            return (T) (object) enumVals[Random.Range(0, enumVals.Count)];
        }


        public static bool GenericTryParse<T>(this string input, out T value)
        {
            TypeConverter converter = TypeDescriptor.GetConverter(typeof(T));

            if (converter.IsValid(input))
            {
                value = (T) converter.ConvertFromString(input);
                return true;
            }

            value = default;
            return false;
        }

        public static T Convert<T>(this string input)
        {
            try
            {
                TypeConverter converter = TypeDescriptor.GetConverter(typeof(T));
                return (T) converter.ConvertFromString(input);
            }
            catch (NotSupportedException)
            {
                return default;
            }
        }

        public static bool TryCast<T>(this object obj, out T result)
        {
            if (obj is T obj1)
            {
                result = obj1;
                return true;
            }

            result = default;
            return false;
        }

        public static bool RunProbability(float chance)
        {
            float random = Random.Range(0f, 1f);

            return random < chance;
        }

        public static Vector3 SnapToPPU(Vector3 unsnappedPos, int PPU)
        {
            float PPUSnap = 1f / PPU;

            Vector3 pos = unsnappedPos;
            pos.x = Mathf.Round(pos.x / PPUSnap) * PPUSnap;
            pos.y = Mathf.Round(pos.y / PPUSnap) * PPUSnap;
            return pos;
        }

        public static void Shuffle<T>(this IList<T> list)
        {
            for (int i = list.Count; i > 0; i--)
                list.Swap(0, Random.Range(0, i));
        }

        public static void Swap<T>(this IList<T> list, int i, int j)
        {
            T temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }

        public static double Normalize(double value, double start, double end)
        {
            double width = end - start;
            double offsetValue = value - start;

            return offsetValue - Mathf.Floor((float) offsetValue / (float) width) * width + start;
        }

        public static Vector3 LerpEuler(Vector3 a, Vector3 b, float t)
        {
            return new Vector3(
                Mathf.LerpAngle(a.x, b.x, t),
                Mathf.LerpAngle(a.y, b.y, t),
                Mathf.LerpAngle(a.z, b.z, t));
        }


        public static Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles)
        {
            return Quaternion.Euler(angles) * (point - pivot) + pivot;
        }


        public static float RoundToDecimal(float value, int decimalPlace)
        {
            float multiplier = Mathf.Pow(10, decimalPlace);

            value *= multiplier;
            value = Mathf.Round(value);
            value /= multiplier;

            return value;
        }

        public static float CeilToDecimal(float value, int decimalPlace)
        {
            float multiplier = Mathf.Pow(10, decimalPlace);

            value *= multiplier;
            value = Mathf.Ceil(value);
            value /= multiplier;

            return value;
        }

        public static float RoundTowardsZeroToDecimal(float value, int decimalPlace)
        {
            float multiplier = Mathf.Pow(10, decimalPlace);

            value *= multiplier;
            value = value < 0 ? Mathf.Ceil(value) : Mathf.Floor(value);
            value /= multiplier;

            return value;
        }

        public static float FloorToDecimal(float value, int decimalPlace)
        {
            float multiplier = Mathf.Pow(10, decimalPlace);

            value *= multiplier;
            value = Mathf.Floor(value);
            value /= multiplier;

            return value;
        }

        public static bool CompareType<T>(T obj1, T obj2)
        {
            return obj1.GetType() == obj2.GetType();
        }

        public static Vector2 GetLocalPosition(Vector2 worldPos, Transform parent)
        {
            return parent.InverseTransformPoint(worldPos);
        }

        public static bool TryGetKey<K, V>(this Dictionary<K, V> dictionary, V value, out K key)
        {
            if (value == null)
            {
                key = default;
                return false;
            }

            foreach (var valuePair in dictionary)
                if (ReferenceEquals(valuePair.Value, value))
                {
                    key = valuePair.Key;
                    return true;
                }

            key = default;
            return false;
        }
        
        public static Vector3 ProjectDirectionOntoNormal(Vector3 dir, Vector3 normal)
        {
            float y = (-normal.x * dir.x - normal.z * dir.z) / normal.y;
            return new Vector3(dir.x, y, dir.z).normalized;
        }

        public static float DirectionToAngle(Vector2 dir)
        {
            return Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        }

        public static Vector3 IgnoreY(this Vector3 vec, bool normalized = false)
        {
            return normalized ? new Vector3(vec.x, 0f, vec.z).normalized : new Vector3(vec.x, 0f, vec.z);
        }

        public static Vector2 To2D(this Vector3 vec, bool normalized = false)
        {
            return normalized ? new Vector2(vec.x, vec.z).normalized : new Vector2(vec.x, vec.z);
        }

        public static Vector3 ProjectOnPlaneRetainMag(Vector3 vector, Vector3 plane)
        {
            return ProjectDirectionOntoNormal(vector.normalized, plane) * vector.magnitude;
        }

        public static T[] GetSubArray<T>(this T[] arr, int startIndex)
        {
            if (arr.Length == 0)
                return arr;
            if (startIndex >= arr.Length)
                return null;
            
            T[] result = new T[arr.Length - startIndex];
            Array.Copy(arr, startIndex, result, 0, result.Length);
            return result;
        }
        
        public static float ClampAngle(float angle, float min, float max)
        {
            if (min < 0 && max > 0 && (angle > max || angle < min))
            {
                angle -= 360;
                if (angle > max || angle < min)
                {
                    return Mathf.Abs(Mathf.DeltaAngle(angle, min)) < 
                           Mathf.Abs(Mathf.DeltaAngle(angle, max)) ? min : max;
                }
            }
            else if(min > 0 && (angle > max || angle < min))
            {
                angle += 360;
                if (angle > max || angle < min)
                {
                    return Mathf.Abs(Mathf.DeltaAngle(angle, min)) < 
                           Mathf.Abs(Mathf.DeltaAngle(angle, max)) ? min : max;
                }
            }
 
            if (angle < min) return min;
            return angle > max ? max : angle;
        }

        public static float NormalizeAngle(float angle)
        {
            while (angle < 0f)
                angle += 360f;

            return angle % 360f;
        }

        public static Vector2 ClampVectorInRadius(Vector2 vector, Vector2 center, float radius)
        {
            Vector2 offset = vector - center;

            return center + Vector2.ClampMagnitude(offset, radius);
        }
        
        public static Vector2 ClampVectorToRadius(Vector2 vector, Vector2 center, float radius)
        {
            Vector2 dir = (vector - center).normalized;

            return center + dir * radius;
        }

        public static bool IsValidIndex<T>(this List<T> list, int index)
        {
            return index >= 0 && index < list.Count;
        }

        public static Vector2 ClampVectorInRadiusRange(Vector2 vector, float innerRadius, float outerRadius)
        {
            float mag = vector.magnitude; 
            if (mag < innerRadius)
                vector = vector.SetMagnitude(innerRadius);
            else if (mag > outerRadius)
                vector = Vector2.ClampMagnitude(vector, outerRadius);
            return vector;
        }

        public static Vector2 ClampVectorInRadiusRange(Vector2 vector, Vector2 center, float innerRadius,
            float outerRadius) => ClampVectorInRadiusRange(vector - center, innerRadius, outerRadius) + center;

        public static Vector2 SetMagnitude(this Vector2 v, float mag) => v.normalized * mag;
        
        public static Vector2 ClampVectorOutsideRadius(Vector2 vector, Vector2 center, float radius)
        {
            float dist = Vector2.Distance(vector, center);
            if (dist < radius)
                return (vector - center).normalized * radius + center;
            return vector;
        }

        public static Vector2 ClampVector(Vector2 vector, Vector2 min, Vector2 max)
        {
            return new Vector2(Mathf.Clamp(vector.x, min.x, max.x), Mathf.Clamp(vector.y, min.y, max.y));
        }

        public static Vector2 RotateDirBy(Vector2 dir, float angleRotation)
        {
            return AngleToDirection(DirectionToAngle(dir) + angleRotation);
        }
        
        public static Vector2 AngleToDirection(float angle)
        {
            return new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
        }

        public static Vector2 GetNormalizedDirectionBetween(Vector2 targetPos, Vector2 myPos)
        {
            return (targetPos - myPos).normalized;
        }

        public static RaycastHit2D CamCast(Camera cam)
        {
            return Physics2D.Raycast(cam.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        }

        public static bool IsPointInCollider(Collider col, Vector3 point)
        {
            return point.Equals(col.ClosestPoint(point));
        }

        public static string GetColoredRichText(string text, Color color)
        {
            return "<color=" + "#" + ColorUtility.ToHtmlStringRGBA(color) + ">" + text + "</color>";
        }

        public static string MakeBold(this string text)
        {
            return "<b>" + text + "</b>";
        }

        public static string MakeUnderlined(this string text)
        {
            return "<u>" + text + "</u>";
        }

        public static Vector2 GetCameraDimensions(Camera camera)
        {
            float cameraHeight = camera.orthographicSize * 2f;
            return new Vector2(cameraHeight * camera.aspect, cameraHeight);
        }

        public static Vector2 GetCameraExtents(Camera camera) => GetCameraDimensions(camera) / 2f;

        public static Color ZeroAlpha(this Color color) => SetAlpha(color, 0f);
        public static Color SetAlpha(this Color color, float alpha) => new Color(color.r, color.g, color.b, alpha);

        public static string ProcessString(this string text, float size, params TextMod[] mods)
        {
            return "<size=" + size + ">" + ProcessString(text, mods);
        }

        public static string ProcessString(this string text, Color color, params TextMod[] mods)
        {
            return ProcessString(GetColoredRichText(text, color), mods);
        }
        
        public static string ProcessString(this string text, Color color, float size, params TextMod[] mods)
        {
            return ProcessString(GetColoredRichText(text, color), size, mods);
        }
        
        public static string ProcessString(this string text, params TextMod[] mods)
        {
            string start = "";
            string end = "";

            foreach (TextMod mod in mods.OrderBy(mod => (int) mod)) 
                switch (mod)
                {
                    case TextMod.Italics:
                        start += "<i>";
                        end += "</i>";
                        break;
                    case TextMod.Bold:
                        start += "<b>";
                        end += "</b>";
                        break;
                    case TextMod.WrapInParentheses:
                        start += "(";
                        end += ")";
                        break;
                    case TextMod.EndInColon:
                        end += ":";
                        break;
                    case TextMod.EndInSpace:
                        end += " ";
                        break;
                    case TextMod.EndInNewLine:
                        end += "\n";
                        break;
                }

            return start + text + end;
        }



        public static bool PointInsideRect(Vector2 dims, Vector2 center, Vector2 point)
        {
            Vector2 extents = dims / 2f;
            return point.x > center.x - extents.x && point.x < center.x + extents.x && point.y < center.y +
                extents.y && point.y > center.y - extents.y;
        }

        public static int GetRandomSign()
        {
            return Math.Sign(Random.Range(-1, 1));
        }

        public static Vector2 GetAvoidanceDirection(Vector2[] avoidancePoints, Vector2 targetTransform)
        {
            int numPoints = avoidancePoints.Length;
            if (numPoints == 0)
                return Vector2.zero;

            float[] angles = new float[numPoints];

            for (int i = 0; i < numPoints; i++)
                angles[i] = NormalizeAngle(DirectionToAngle(avoidancePoints[i] - targetTransform));

            if (numPoints == 1)
                return -AngleToDirection(angles[0]);
            
            angles = angles.OrderBy(angle => angle).ToArray();

            int greatestAngleDiffIndex = 0;
            float greatestAngleDiff = 0f;

            for (int i = 1; i <= numPoints; i++)
            {
                int index = i < numPoints ? i : 0;
                float angle1 = angles[index];
                float angle2 = angles[i - 1];
                float angleDiff = NormalizeAngle(angle1 - angle2);//Mathf.DeltaAngle(angles[index], angles[i - 1]);
                //Debug.Log($"angle1: {angle1}, angle2: {angle2}, angleDiff: {angleDiff}");
                
                if (Mathf.Abs(angleDiff) > Mathf.Abs(greatestAngleDiff))
                {
                    greatestAngleDiff = angleDiff;
                    greatestAngleDiffIndex = index;
                }
            }

            int index2 = greatestAngleDiffIndex > 0 ? greatestAngleDiffIndex - 1 : numPoints - 1;
            return AngleToDirection(NormalizeAngle(angles[greatestAngleDiffIndex] - (greatestAngleDiff/2f)));
        }
        
        public static Vector2? LSegsIntersectionPoint(Vector2 ps1, Vector2 pe1, Vector2 ps2, Vector2 pe2)
        {
            // Get A,B of first line - points : ps1 to pe1
            float A1 = pe1.y-ps1.y;
            float B1 = ps1.x-pe1.x;
            // Get A,B of second line - points : ps2 to pe2
            float A2 = pe2.y-ps2.y;
            float B2 = ps2.x-pe2.x;

            // Get delta and check if the lines are parallel
            float delta = A1*B2 - A2*B1;
            if(delta == 0) return null;

            // Get C of first and second lines
            float C2 = A2*ps2.x+B2*ps2.y;
            float C1 = A1*ps1.x+B1*ps1.y;
            //invert delta to make division cheaper
            float invdelta = 1/delta;
            // now return the Vector2 intersection point
            return new Vector2( (B2*C1 - B1*C2)*invdelta, (A1*C2 - A2*C1)*invdelta );
        }
        
        public static Vector2? LSegRec_IntersPoint_v01(Vector2 p1, Vector2 p2, Vector2 r1, Vector2 r2, Vector2 r3, Vector2 r4)
        {
            Vector2? intersection = null;
            intersection = LSegsIntersectionPoint(p1,p2,r1,r2);
            if(intersection == null) intersection = LSegsIntersectionPoint(p1,p2,r2,r3);
            if(intersection == null) intersection = LSegsIntersectionPoint(p1,p2,r3,r4);
            if(intersection == null) intersection = LSegsIntersectionPoint(p1,p2,r4,r1);
            return intersection;
        }
        
        private static float Cross(Vector2 v1, Vector2 v2)
        {
            return v1.x * v2.y - v1.y * v2.x;
        }

        private static float Mult(Vector2 v1, Vector2 v2)
        {
            return v1.x * v2.x + v1.y * v2.y;
        }
        
        public static bool LineSegmentIntersect(in Vector2 p, in Vector2 p2, in Vector2 q, in Vector2 q2, 
            out Vector2 intersection, bool considerCollinearOverlapAsIntersect = false)
        {
            intersection = default;

            Vector2 r = p2 - p;
            Vector2 s = q2 - q;
            float rxs = Cross(r, s);
            float qpxr = Cross(q - p, r);

            bool rxsZero = Mathf.Approximately(rxs, 0f);
            bool qpxrZero = Mathf.Approximately(qpxr, 0f);
            
            // If r x s = 0 and (q - p) x r = 0, then the two lines are collinear.
            if (rxsZero && qpxrZero)
            {
                // 1. If either  0 <= (q - p) * r <= r * r or 0 <= (p - q) * s <= * s
                // then the two lines are overlapping,
                if (considerCollinearOverlapAsIntersect)
                    if ((0 <= Mult((q - p), r) && (Mult(q - p, r) <= Mult(r, r) || (0 <= Mult(p - q, s) && Mult(p - q, s) <= Mult(s, s)))))
                        return true;

                // 2. If neither 0 <= (q - p) * r = r * r nor 0 <= (p - q) * s <= s * s
                // then the two lines are collinear but disjoint.
                // No need to implement this expression, as it follows from the expression above.
                return false;
            }

            // 3. If r x s = 0 and (q - p) x r != 0, then the two lines are parallel and non-intersecting.
            if (rxsZero && !qpxrZero)
                return false;

            // t = (q - p) x s / (r x s)
            float t = Cross(q - p, s/rxs);

            // u = (q - p) x r / (r x s)

            float u = Cross(q - p, r/rxs);

            // 4. If r x s != 0 and 0 <= t <= 1 and 0 <= u <= 1
            // the two line segments meet at the point p + t r = q + u s.
            if (!rxsZero && (0 <= t && t <= 1) && (0 <= u && u <= 1))
            {
                // We can calculate the intersection point using either t or u.
                intersection = p + t*r;

                // An intersection was found.
                return true;
            }

            // 5. Otherwise, the two line segments are not parallel but do not intersect.
            return false;
        }

        public static bool LineSegIntersectsRect(Vector2 topLeft, Vector2 topRight, Vector2 bottomRight,
            Vector2 bottomLeft, Vector2 p1, Vector2 p2,
            out Vector2 intersection, bool considerCollinearOverlapAsIntersect = false)
        {
            if (LineSegmentIntersect(topLeft, topRight, p1, p2, out intersection,
                considerCollinearOverlapAsIntersect))
                return true;
            if (LineSegmentIntersect(topRight, bottomRight, p1, p2, out intersection,
                considerCollinearOverlapAsIntersect))
                return true;
            if (LineSegmentIntersect(bottomRight, bottomLeft, p1, p2, out intersection,
                considerCollinearOverlapAsIntersect))
                return true;
            if (LineSegmentIntersect(bottomLeft, topLeft, p1, p2, out intersection,
                considerCollinearOverlapAsIntersect))
                return true;
            return false;
        }

        public static string ParseFloat(float f, int numDecimals, int preferredNumCount)
        {
            string floatStr = f.ToString();

            string returnStr = "";

            int decimalCount = 0, numCount = 0;
            bool foundDecimal = false;

            foreach (char c in floatStr)
            {
                if (c == '.')
                {
                    if (numCount >= preferredNumCount)
                        break;
                    
                    foundDecimal = true;
                }
                else
                {
                    if (foundDecimal)
                        decimalCount++;
                    
                    numCount++;
                }
                
                returnStr += c;

                if (numCount == preferredNumCount && foundDecimal || decimalCount == numDecimals)
                    break;
            }

            return returnStr;
        }

        public static bool Overlaps(this RectTransform a, RectTransform b) {
            return a.WorldRect().Overlaps(b.WorldRect());
        }
        public static bool Overlaps(this RectTransform a, RectTransform b, bool allowInverse) {
            return a.WorldRect().Overlaps(b.WorldRect(), allowInverse);
        }

        public static Rect WorldRect(this RectTransform rectTransform) {
            Vector2 sizeDelta = rectTransform.sizeDelta;
            float rectTransformWidth = sizeDelta.x * rectTransform.lossyScale.x;
            float rectTransformHeight = sizeDelta.y * rectTransform.lossyScale.y;

            Vector3 position = rectTransform.position;
            return new Rect(position.x - rectTransformWidth / 2f, position.y - rectTransformHeight / 2f,
                rectTransformWidth, rectTransformHeight);
        }
        
        public static IEnumerable<(T item, int index)> WithIndex<T>(this IEnumerable<T> source)
        {
            return source.Select((item, index) => (item, index));
        }

        public static Vector3 SetX(this Vector3 v, float newX) => new Vector3(newX, v.y, v.z);
        public static Vector3 SetY(this Vector3 v, float newY) => new Vector3(v.x, newY, v.z);
        public static Vector3 SetZ(this Vector3 v, float newZ) => new Vector3(v.x, v.y, newZ);
        public static Vector3Int ToInt(this Vector3 v) => new Vector3Int((int)v.x, (int)v.y, (int)v.z);
        public static Vector2Int ToInt(this Vector2 v) => new Vector2Int((int)v.x, (int)v.y);
        public static Vector2 SetX(this Vector2 v, float newX) => new Vector2(newX, v.y);
        public static Vector2 SetY(this Vector2 v, float newY) => new Vector2(v.x, newY);

        public static List<Vector3> ToV3s(this List<Vector2> vectors2s) =>
            GetElementFromAll(vectors2s, vector2 => (Vector3) vector2); 

        public static Vector2 ScreenDims => new Vector2(Screen.width, Screen.height);

        public static bool ScreenContainsScreenPoint(Vector2 pos) =>
            PointInsideRect(ScreenDims, ScreenDims/2f, pos);

        public static bool MouseInWindow() => ScreenContainsScreenPoint(Input.mousePosition);

        public static Vector2Int FloorVector(this Vector2 vector) => new Vector2Int(Mathf.FloorToInt(vector.x),
            Mathf.FloorToInt(vector.y));

        public static bool IsOfEnumType<T>(this T enumerable, params T[] types) where T : Enum
        {
            foreach (T type in types)
            {
                if (Equals(enumerable, type))
                    return true;
            }

            return false;
        }
        
        public static Vector2 FlipX(this Vector2 vector) => FlipX((Vector3)vector);
        public static Vector2 FlipY(this Vector2 vector) => FlipY((Vector3)vector);
        public static Vector2 Flip(this Vector2 vector) => Flip((Vector3)vector);
        public static Vector3 FlipX(this Vector3 vector) => new Vector3(vector.x * -1f, vector.y, vector.z);
        public static Vector3 FlipY(this Vector3 vector) => new Vector3(vector.x, vector.y * -1f, vector.z);
        public static Vector3 FlipZ(this Vector3 vector) => new Vector3(vector.x, vector.y * -1f, vector.z);
        
        public static Vector3 Flip(this Vector3 vector) => new Vector3(vector.x * -1f, vector.y * -1f, vector.z * -1f);

        public static void DebugAll<T>(this List<T> list, Func<T, string> property)
        {
            foreach (T element in list)
                Debug.Log(property(element));
        }

        public static string GetEnumeratedString<T>(this List<T> list, Func<T, string> property)
        {
            string returnStr = "";
            foreach (var (element, index) in list.WithIndex())
            {
                returnStr += property(element);
                if (index != list.Count - 1)
                    returnStr += ", ";
            }

            return returnStr;
        }

        public static List<E> GetElementFromAll<L, E>(this IEnumerable<L> list, Func<L, E> property)
        {
            List<E> elements = new List<E>();
            foreach (L element in list)
                elements.Add(property(element));
            return elements;
        }
        
        public static List<E> GetElementFromAll<L, E>(this IEnumerable<L> list, Func<L, bool> condition, Func<L, E> property)
        {
            List<E> elements = new List<E>();
            foreach (L element in list)
                if (condition(element))
                    elements.Add(property(element));

            return elements;
        }

        public static IEnumerable<E> GetAllThatMeetCondition<E>(this IEnumerable<E> list, Func<E, bool> condition)
        {
            List<E> elements = new List<E>();
            foreach (E element in list)
                if (condition(element))
                    elements.Add(element);

            return elements;
        }

        public static List<T> AppendElementAndReturnCopy<T>(this List<T> list, T newElement)
        {
            List<T> newList = new List<T>(list);
            newList.Add(newElement);
            return newList;
        }
        
        public static List<T> ReturnCopyWithCutoffEnd<T>(this List<T> list, int numElementsToRemove)
        {
            List<T> newList = new List<T>(list);
            int count = 0;
            while (newList.Count > 0 && count < numElementsToRemove)
            {
                newList.RemoveAt(newList.Count - 1);
                count++;
            }

            return newList;
        }

        public static T GetMinWithProp<T>(this IEnumerable<T> list, Func<T, float> property)
        {
            float minProp = Mathf.Infinity;
            T min = default;
            foreach (T element in list)
            {
                float prop = property(element);
                if (prop < minProp)
                {
                    minProp = prop;
                    min = element;
                }
            }

            return min;
        }
        
        public static T GetMaxWithProp<T>(this IEnumerable<T> list, Func<T, float> property)
        {
            float maxProp = Mathf.NegativeInfinity;
            T max = default;
            foreach (T element in list)
            {
                float prop = property(element);
                if (prop > maxProp)
                {
                    maxProp = prop;
                    max = element;
                }
            }

            return max;
        }
        
        public static float MinDistToLineSeg(Vector2 A, Vector2 B, Vector2 E)
        {
            // vector AB
            Vector2 AB = new Vector2();
            AB.x = B.x - A.x;
            AB.y = B.y - A.y;
 
            // vector BP
            Vector2 BE = new Vector2();
            BE.x = E.x - B.x;
            BE.y = E.y - B.y;
 
            // vector AP
            Vector2 AE = new Vector2();
            AE.x = E.x - A.x;
            AE.y = E.y - A.y;
 
            // Variables to store dot product
            float AB_BE, AB_AE;
 
            // Calculating the dot product
            AB_BE = (AB.x * BE.x + AB.y * BE.y);
            AB_AE = (AB.x * AE.x + AB.y * AE.y);
 
            // Minimum distance from
            // point E to the line segment
            float reqAns = 0;
 
            // Case 1
            if (AB_BE > 0)
            {
 
                // Finding the magnitude
                float y = E.y - B.y;
                float x = E.x - B.x;
                reqAns = Mathf.Sqrt(x * x + y * y);
            }
 
            // Case 2
            else if (AB_AE < 0)
            {
                float y = E.y - A.y;
                float x = E.x - A.x;
                reqAns = Mathf.Sqrt(x * x + y * y);
            }
 
            // Case 3
            else
            {
 
                // Finding the perpendicular distance
                float x1 = AB.x;
                float y1 = AB.y;
                float x2 = AE.x;
                float y2 = AE.y;
                float mod = Mathf.Sqrt(x1 * x1 + y1 * y1);
                reqAns = Mathf.Abs(x1 * y2 - y1 * x2) / mod;
            }
            return reqAns;
        }

        public static void GetMinAndMax(Vector2Int corner1, Vector2Int corner2, 
            out int minX, out int minY, out int maxX, out int maxY)
        {
            minX = Mathf.Min(corner1.x, corner2.x);
            maxX = Mathf.Max(corner1.x, corner2.x);
            minY = Mathf.Min(corner1.y, corner2.y);
            maxY = Mathf.Max(corner1.y, corner2.y);
        }
        public static IEnumerable<Vector2Int> IterateOverRect(Vector2Int corner1, Vector2Int corner2, bool borderOnly = false)
        {
            GetMinAndMax(corner1, corner2, out int minX, out int minY, out int maxX, out int maxY);

            return IterateOverRect(minX, minY, maxX, maxY, borderOnly);
        }

        public static IEnumerable<Vector2Int> IterateOverRect(int minX, int minY, int maxX, int maxY,
            bool borderOnly = false)
        {
            foreach (Vector2Int pos in GenerateRectList(minX, minY, maxX, maxY, borderOnly))
            {
                yield return pos;
            }
        }

        public static List<Vector2Int> GenerateRectList(int minX, int minY, int maxX, int maxY,
            bool borderOnly = false)
        {
            List<Vector2Int> points = new List<Vector2Int>();
            int rectWidth = maxX - minX;
            
            for (int y = minY; y <= maxY; y++)
            {
                int xStepSize;
                for (int x = minX; x <= maxX; x += xStepSize)
                {
                    if (borderOnly && y > minY && y < maxY)
                        xStepSize = Mathf.Max(rectWidth, 1);
                    else
                        xStepSize = 1;
                    points.Add(new Vector2Int(x, y));
                }
            }

            return points;
        }

        public static float Abs(this float f) => Mathf.Abs(f);
        public static int Abs(this int f) => Mathf.Abs(f);

        public static Vector2 FindBoundsOfSetOfPoints(List<Vector2> points)
        {
            float biggestX = Mathf.NegativeInfinity, biggestY = Mathf.NegativeInfinity;
            float smallestX = Mathf.Infinity, smallestY = Mathf.Infinity;

            foreach (Vector2 point in points)
            {
                if (point.x > biggestX)
                    biggestX = point.x;
                if (point.y > biggestY)
                    biggestY = point.y;
                if (point.x < smallestX)
                    smallestX = point.x;
                if (point.y < smallestY)
                    smallestY = point.y;
            }
            return new Vector2(biggestX - smallestX, biggestY - smallestY);
        }
        
        public static Vector2Int FindBoundsOfSetOfPoints(List<Vector2Int> points)
        {
            int biggestX = int.MinValue, biggestY = int.MinValue;
            int smallestX = int.MaxValue, smallestY = int.MaxValue;

            foreach (Vector2Int point in points)
            {
                if (point.x > biggestX)
                    biggestX = point.x;
                if (point.y > biggestY)
                    biggestY = point.y;
                if (point.x < smallestX)
                    smallestX = point.x;
                if (point.y < smallestY)
                    smallestY = point.y;
            }
            return new Vector2Int(biggestX - smallestX + 1, biggestY - smallestY + 1);
        }
        
        public static float GetFinalVelocity(float aVelocityChange, float aDrag)
        {
            return aVelocityChange * (1 / Mathf.Clamp01(aDrag * Time.fixedDeltaTime) - 1);
        }
        public static float GetDrag(float aVelocityChange, float aFinalVelocity)
        {
            return aVelocityChange / ((aFinalVelocity + aVelocityChange) * Time.fixedDeltaTime);
        }
        
        public static float GetDragFromAcceleration(float aAcceleration, float aFinalVelocity)
        {
            return GetDrag(aAcceleration * Time.fixedDeltaTime, aFinalVelocity);
        }
        
        public static float GetRequiredVelocityChange(float aFinalSpeed, float aDrag)
        {
            float m = Mathf.Clamp01(aDrag * Time.fixedDeltaTime);
            return aFinalSpeed * m / (1 - m);
        }
    }

    public enum TextMod
    {
        WrapInParentheses, 
        EndInColon,
        EndInSpace,
        EndInNewLine,
        Italics,
        Bold,
    }
}