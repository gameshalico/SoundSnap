using UnityEditor;
using UnityEngine.UIElements;

namespace SoundSnap.Editor
{
    [CustomEditor(typeof(SoundAgent))]
    public class SoundAgentEditor : UnityEditor.Editor
    {
        private Toggle _isPlayingField;
        private IntegerField _loopCountField;
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
            var soundPlayer = (SoundAgent)target;
            _stateEnumField.SetValueWithoutNotify(soundPlayer.CurrentPlaybackState);
            _versionField.SetValueWithoutNotify(soundPlayer.Version);
            _playDspTimeField.SetValueWithoutNotify(soundPlayer.PlayDspTime);
            _isPlayingField.SetValueWithoutNotify(soundPlayer.IsPlaying);
            _timeField.SetValueWithoutNotify(soundPlayer.Time);
            _timeSamplesField.SetValueWithoutNotify(soundPlayer.TimeSamples);
            _loopCountField.SetValueWithoutNotify(soundPlayer.LoopCount);
        }


        public override VisualElement CreateInspectorGUI()
        {
            var container = new VisualElement();

            _stateEnumField = new EnumField("Playback State", SoundAgent.PlaybackState.Free);
            _versionField = new IntegerField("Version");
            _playDspTimeField = new DoubleField("Dsp Time");
            _isPlayingField = new Toggle("Is Playing");
            _timeField = new FloatField("Time");
            _timeSamplesField = new IntegerField("Time Samples");
            _loopCountField = new IntegerField("Loop Count");

            _stateEnumField.SetEnabled(false);
            _versionField.SetEnabled(false);
            _playDspTimeField.SetEnabled(false);
            _isPlayingField.SetEnabled(false);
            _timeField.SetEnabled(false);
            _timeSamplesField.SetEnabled(false);
            _loopCountField.SetEnabled(false);

            UpdateValues();

            container.Add(_stateEnumField);
            container.Add(_versionField);
            container.Add(_playDspTimeField);
            container.Add(_isPlayingField);
            container.Add(_timeField);
            container.Add(_timeSamplesField);
            container.Add(_loopCountField);

            return container;
        }
    }
}