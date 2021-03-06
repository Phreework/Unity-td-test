﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugTool : MonoBehaviour {

    public static bool IsAndroid
        {
            get
            {
                bool retValue = false;
#if UNITY_ANDROID
                retValue = true;    
#endif
                return retValue;
            }
        }

        public static bool IsEditor
        {
            get
            {
                bool retValue = false;
#if UNITY_EDITOR
    retValue = true;    
#endif
                return retValue;
            }
        }

        public static bool IsiOS
        {
            get
            {
                bool retValue = false;
#if UNITY_IOS
                retValue = true;    
#endif
                return retValue;
            }
        }
}
