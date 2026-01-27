using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;


public class WebGLPluginJS : MonoBehaviour
{
	[DllImport("__Internal")]
	public static extern void ReactQuitRequest();

	[DllImport("__Internal")]
	public static extern void Closewindow();

	[DllImport("__Internal")]
	public static extern void Redirect(string url);

	[DllImport("__Internal")]
	public static extern void SessionRedirect(string sessionStorageItem);
}
