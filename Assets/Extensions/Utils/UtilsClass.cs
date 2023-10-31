using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

/// <summary>
/// Written by Pierre Mizzi
/// </summary>
namespace PierreMizzi.Useful
{
    public static class UtilsClass
    {
        #region Maths

        public static Quaternion k_flipQuaternion = Quaternion.Euler(0f, 180f, 0f);

        public static float GRAVITY = -9.81f;
        public static float Tau
        {
            get { return Mathf.PI * 2f; }
        }

        public static float UnclampedLerp(float a, float b, float t)
        {
            return t * b + (1 - t) * a;
        }

        public static Vector3 UnclampedLerp(Vector3 a, Vector3 b, float t)
        {
            return t * b + (1 - t) * a;
        }

        public static float Clamp01(float value)
        {
            return Mathf.Max(0, Mathf.Min(value, 1));
        }

        public static float RandomFromNegativePositive(int value)
        {
            return Random.Range(-Mathf.Abs(value), Mathf.Abs(value));
        }

        public static float RandomFromNegativePositive(float value)
        {
            return Random.Range(-Mathf.Abs(value), Mathf.Abs(value));
        }

        public static float RandomFromRange(Vector2 range)
        {
            return Random.Range(range.x, range.y);
        }

        public static int RandomFromRange(Vector2Int range)
        {
            return Random.Range(range.x, range.y);
        }

        public static bool FlipCoin()
        {
            return UnityEngine.Random.Range(0, 2) == 0;
        }

        public static float GetNoiseValue(float value, int offset = 0)
        {
            float f = value % 1.0f;
            value = Mathf.Floor(value);
            return Mathf.Lerp(
                UtilsClass.GetRandom(value),
                UtilsClass.GetRandom(value + 1.0f),
                Mathf.SmoothStep(0.0f, 1.0f, f)
            );
        }

        public static float GetRandom(float value)
        {
            return (Mathf.Sin(value) * 10000.0f) % 1.0f;
        }

        public static float MinusPlusToZeroPlus(float value)
        {
            return (value + 1.0f) * 0.5f;
        }

        public static float ZeroPlusToMinusPlus(float value)
        {
            return (value * 2.0f) - 1.0f;
        }

        public static float ToFullAngle(float angle)
        {
            if (angle > 0)
                return angle;
            else
                return 360.0f + angle;
        }

        public static float OffsetFullAngle(float angle, float offset)
        {
            if (offset < 0)
            {
                if (angle + offset < 0)
                    return 360 + angle + offset;
                else
                    return angle + offset;
            }
            else
            {
                if (angle + offset > 360)
                    return angle + offset - 360;
                else
                    return angle + offset;
            }
        }

        public static Vector3 FlattenVector(Vector3 vector)
        {
            return new Vector3(vector.x, 0, vector.z);
        }

        public static float Frac(float value)
        {
            return value - Mathf.Floor(value);
        }

        public static Vector2 MultiplyVector(Vector2 a, Vector2 b)
        {
            return new Vector2(a.x * b.x, a.y * b.y);
        }

        public static Vector3 MultiplyVector(Vector3 a, Vector3 b)
        {
            return new Vector3(a.x * b.x, a.y * b.y, a.z * b.z);
        }

        public static float Remap(float value, float from1, float to1, float from2, float to2)
        {
            return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
        }

        #endregion

        public static void FillListFromType<T>(Transform container, List<T> list)
        {
            foreach (Transform child in container)
            {
                if (child.TryGetComponent(out T element))
                {
                    if (child.gameObject.activeInHierarchy && !list.Contains(element))
                        list.Add(element);
                }
            }
        }

        public static void WriteFile(byte[] data, string localURL)
        {
            Debug.Log($"WriteFile : {localURL}");
            int lastSlashIndex = localURL.LastIndexOf(@"/");
            string directory = localURL.Substring(0, lastSlashIndex);

            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            File.WriteAllBytes(localURL, data);
        }

        public static byte[] StringToByteArray(string data)
        {
            return Encoding.ASCII.GetBytes(data);
        }

        public static string TwoDigit(string data)
        {
            if (data.Length == 1)
                return "0" + data;
            else if (data == "0")
                return "00";
            else
                return data;
        }

        public static string TwoDigit(object data)
        {
            return TwoDigit(data.ToString());
        }

        public static string CurrentSceneName
        {
            get { return UnityEngine.SceneManagement.SceneManager.GetActiveScene().name; }
        }

        public static int TextTimeToValueTime(string textTime)
        {
            int min = 0;
            int sec = 0;

            string minStr = Regex.Match(textTime, @"(\d*):").Value;
            minStr = minStr.Replace(":", "");
            min = int.Parse(minStr);

            string secStr = Regex.Match(textTime, @":(\d*)").Value;
            secStr = secStr.Replace(":", "");
            sec = int.Parse(secStr);

            return (min + (int)Mathf.Floor(sec / 60.0f)) * 60 + sec % 60;
        }

        public static string SecondsToTextTime(double totalSeconds)
        {
            int minutes = (int)(totalSeconds / 60d);
            int seconds = (int)(totalSeconds % 60d);

            return minutes + ":" + TwoDigit(seconds);
        }

        public static string GetLastChar(string value)
        {
            return value[value.Length - 1].ToString();
        }

        public static T PickRandomInList<T>(List<T> list)
        {
            if (list.Count == 0)
                return default(T);
            else
                return list[Random.Range(0, list.Count)];
        }

        public static T PickRandom<T>(this List<T> list)
        {
            if (list.Count == 0)
                return default(T);
            else
                return list[Random.Range(0, list.Count)];
        }

        // Vector3 because Input.mousePosition also is ... don't ask me why
        public static Vector3 MiddleScreenPosition
        {
            get { return new Vector3(Screen.width / 2f, Screen.height / 2f, 0f); }
        }

        public static Vector3 RandomDirection()
        {
            return new Vector3(Random.Range(-1f, 1),
                               Random.Range(-1f, 1f),
                               Random.Range(-1f, 1f)).normalized;
        }

        public static Vector2 RandomDirection2D()
        {
            return new Vector2(Random.Range(-1f, 1),
                               Random.Range(-1f, 1f)).normalized;
        }

        public static Vector3 RandomPosition(this Bounds bounds)
        {
            return new Vector3(
                bounds.center.x + Random.Range(-bounds.extents.x, bounds.extents.x),
                bounds.center.y + Random.Range(-bounds.extents.y, bounds.extents.y),
                bounds.center.z + Random.Range(-bounds.extents.z, bounds.extents.z)
            );
        }

        public static Vector2 RandomPosition2D(this Bounds bounds)
        {
            return new Vector2(
                bounds.center.x + Random.Range(-bounds.extents.x, bounds.extents.x),
                bounds.center.y + Random.Range(-bounds.extents.y, bounds.extents.y)
            );
        }

        public static Quaternion RandomRotation()
        {
            return Quaternion.Euler(
                RandomAngle(),
                RandomAngle(),
                RandomAngle()
            );
        }

        public static Quaternion RandomRotation2D()
        {
            return Quaternion.Euler(
                0f,
                0f,
                RandomAngle()
            );
        }

        public static float RandomAngle()
        {
            return Random.Range(0f, 360f);
        }

        public static Quaternion RandomRotation(Vector3 minRotation, Vector3 maxRotation)
        {
            return Quaternion.Euler(
                Random.Range(minRotation.x, maxRotation.x),
                Random.Range(minRotation.y, maxRotation.y),
                Random.Range(minRotation.z, maxRotation.z)
            );
        }

        #region Visual

        public static string MainTexProperty = "_MainTex";
        public static string TintProperty = "_Tint";
        public static string ColorProperty = "_Color";

        /// <summary>
        /// Color(1,1,1,0);
        /// </summary>
        public static Color Transparent = new Color(1, 1, 1, 0);

        public static Color Orange = new Color(1f, .5f, .1f);

        public static Color ChangeAlpha(Color color, float alpha)
        {
            Color col = color;
            return new Color(col.r, col.g, col.b, alpha);
        }

        #endregion

        #region Monobehaviour

        public static void EmptyTransform(Transform transform, bool isImmediate = true)
        {
            if (transform.childCount == 0)
                return;
            for (int i = transform.childCount; i > 0; --i)
            {
                if (isImmediate)
                    GameObject.DestroyImmediate(transform.GetChild(0).gameObject);
                else
                    GameObject.Destroy(transform.GetChild(0).gameObject);
            }
        }

        #endregion

        #region Collision

        /// <summary>
        /// Checks if layerValueB is inside layerValueA masks
        /// </summary>
        /// <param name="layerValueA">value of the layer mask</param>
        /// <param name="layerValueB">value of the layer to check within layerMaskValue</param>
        public static bool CheckLayer(int layerMaskValue, int layerToCheck)
        {
            return (layerMaskValue & 1 << layerToCheck) != 0;
        }

        #endregion

        #region Bitwise Operation

        public static int SetFlag(int a, int b)
        {
            return a | b;
        }

        public static int UnsetFlag(int a, int b)
        {
            return a & (~b);
        }

        public static bool HasFlag(int a, int b)
        {
            return (a & b) == b;
        }

        public static bool HasFlags(int a, params int[] flags)
        {
            foreach (int flag in flags)
            {
                if (HasFlag(a, flag))
                    return true;
            }
            return false;
        }

        #endregion

        #region Regex

        public static string lineBreak = " \r\n";

        public static bool IsValidFilename(string testName)
        {
            if (string.IsNullOrEmpty(testName))
                return false;
            else
                return Regex.IsMatch(testName, "^[^\\/?%*:|\"<>.]+$");
        }

        /// <summary>
        /// Splits path into various useful elements
        /// - Group 1 : Path
        /// - Group 2 : File name
        /// - Group 3 : File extension
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static GroupCollection RegexPathElements(string path)
        {
            return Regex.Match(path, @"^/?(.+/)*(.+)\.(.+)$").Groups;
        }

        public static string RegexPath(string path)
        {
            GroupCollection groups = RegexPathElements(path);

            if (groups.Count > 0)
                return groups[1].Value;
            else
                return "";
        }

        public static string RegexFileName(string path)
        {
            GroupCollection groups = RegexPathElements(path);

            if (groups.Count > 0)
                return groups[2].Value;
            else
                return "";
        }

        public static string RegexFileExtension(string path)
        {
            GroupCollection groups = RegexPathElements(path);

            if (groups.Count > 0)
                return groups[3].Value;
            else
                return "";
        }

        public static string RegexPascalCaseToText(string text)
        {
            return Regex.Replace(text, "(\\B[A-Z])", " $1");
        }

        #endregion
    }
}
