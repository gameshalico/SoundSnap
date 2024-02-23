using UnityEditor;
using UnityEngine.UIElements;

namespace SoundSnap.Editor
{
    [CustomEditor(typeof(SoundPool))]
    public class SoundPoolEditor : UnityEditor.Editor
    {
        private IntegerField _freePlayerCountField;
        private IntegerField _maxPlayerCountField;
        private IntegerField _playerCountField;

        private void OnEnable()
        {
            EditorApplication.update += UpdateValues;
        }

        private void OnDisable()
        {
            EditorApplication.update -= UpdateValues;
        }

        private void UpdateValues()
        {
            var soundPool = (SoundPool)target;

            _freePlayerCountField.SetValueWithoutNotify(soundPool.FreePlayerCount);
            _playerCountField.SetValueWithoutNotify(soundPool.PlayerCount);
            _maxPlayerCountField.SetValueWithoutNotify(SoundSnapSettings.Instance.MaxSoundPlayerCount);
        }

        public override VisualElement CreateInspectorGUI()
        {
            var container = new VisualElement();


            _freePlayerCountField = new IntegerField("Free Player Count");
            _playerCountField = new IntegerField("Player Count");
            _maxPlayerCountField = new IntegerField("Max Player Count");

            _freePlayerCountField.SetEnabled(false);
            _playerCountField.SetEnabled(false);
            _maxPlayerCountField.SetEnabled(false);

            container.Add(_freePlayerCountField);
            container.Add(_playerCountField);
            container.Add(_maxPlayerCountField);

            UpdateValues();

            return container;
        }
    }
}