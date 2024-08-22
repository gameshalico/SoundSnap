using UnityEditor;
using UnityEngine.UIElements;

namespace SoundSnap.Editor
{
    [CustomEditor(typeof(SnapPlayer))]
    public class SnapPlayerEditor : UnityEditor.Editor
    {
        private Toggle _isPlayingField;
        private DoubleField _playDspTimeField;
        private EnumField _stateEnumField;
        private FloatField _timeField;
        private IntegerField _timeSamplesField;
        private IntegerField _versionField;

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
            var soundPlayer = (SnapPlayer)target;
            _stateEnumField.SetValueWithoutNotify(soundPlayer.State);
            _versionField.SetValueWithoutNotify(soundPlayer.Version);
            _playDspTimeField.SetValueWithoutNotify(soundPlayer.PlayDspTime);
            _isPlayingField.SetValueWithoutNotify(soundPlayer.IsPlaying);
            _timeField.SetValueWithoutNotify(soundPlayer.Time);
            _timeSamplesField.SetValueWithoutNotify(soundPlayer.TimeSamples);
        }


        public override VisualElement CreateInspectorGUI()
        {
            var container = new VisualElement();

            _stateEnumField = new EnumField("Playback State", PlaybackState.Free);
            _versionField = new IntegerField("Version");
            _playDspTimeField = new DoubleField("Dsp Time");
            _isPlayingField = new Toggle("Is Playing");
            _timeField = new FloatField("Time");
            _timeSamplesField = new IntegerField("Time Samples");

            _stateEnumField.SetEnabled(false);
            _versionField.SetEnabled(false);
            _playDspTimeField.SetEnabled(false);
            _isPlayingField.SetEnabled(false);
            _timeField.SetEnabled(false);
            _timeSamplesField.SetEnabled(false);

            UpdateValues();

            container.Add(_stateEnumField);
            container.Add(_versionField);
            container.Add(_playDspTimeField);
            container.Add(_isPlayingField);
            container.Add(_timeField);
            container.Add(_timeSamplesField);

            return container;
        }
    }
}