using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

[CreateAssetMenu(fileName = "TutorialsTextDatabase", menuName = "ScriptableObjects/Tutorial/TutorialTextsContainer")]
public class TutorialTextScriptableObject : ScriptableObject
{
    [SerializeField] public LocalizedString[] tutorial1Texts;
    [SerializeField] public LocalizedString[] tutorial2Texts;
}
