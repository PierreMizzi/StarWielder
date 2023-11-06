using System.Collections.Generic;
using UnityEngine;


namespace PierreMizzi.Extensions.CursorManagement
{

    public delegate void CursorDelegate(CursorType type);

    /// <summary>
    /// Handles cursor's properties in-game when fighting or in menu
    /// </summary>
    public class CursorManager : MonoBehaviour
    {

        [SerializeField] private CursorChannel m_cursorChannel;
        [SerializeField] private List<CursorConfig> m_configs = new List<CursorConfig>();

        /// <summary>
        /// Associate cursor's configurations to one type of cursor
        /// </summary>
        private Dictionary<CursorType, CursorConfig> m_typeToConfig = new Dictionary<CursorType, CursorConfig>();

        private void Start()
        {
            foreach (CursorConfig config in m_configs)
            {
                if (!m_typeToConfig.ContainsKey(config.type))
                    m_typeToConfig.Add(config.type, config);
            }

            if (m_cursorChannel != null)
                m_cursorChannel.onSetCursor += SetCursor;

        }

        private void OnDestroy()
        {
            if (m_cursorChannel != null)
                m_cursorChannel.onSetCursor -= SetCursor;

        }

        /// <summary>
        /// Changes cursor propreties, like texture or hotspot
        /// </summary>
        /// <param name="type"></param>
        private void SetCursor(CursorType type)
        {
            if (m_typeToConfig.ContainsKey(type))
            {
                CursorConfig config = m_typeToConfig[type];
                Cursor.SetCursor(config.texture, config.hotspot, config.mode);
            }
        }

    }
}