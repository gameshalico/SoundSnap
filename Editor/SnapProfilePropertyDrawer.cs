using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace SoundSnap.Editor
{
    [CustomPropertyDrawer(typeof(SnapProfile))]
    public class SnapProfilePropertyDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var container = new VisualElement();

            var foldout = new Foldout { text = property.displayName };

            var clipProperty = property.FindPropertyRelative("_clip");
            var outputAudioMixerGroupProperty = property.FindPropertyRelative("_outputAudioMixerGroup");
            var muteProperty = property.FindPropertyRelative("_mute");
            var startSampleProperty = property.FindPropertyRelative("_startSample");

            var audioClipField = new PropertyField(clipProperty) { label = "AudioClip" };

            container.Add(audioClipField);
            container.Add(new PropertyField(outputAudioMixerGroupProperty) { label = "Output" });
            container.Add(new PropertyField(muteProperty));

            AddSliderFields(container, property);
            AddSamplesFields(container, property);
            AddTimingFields(container, property);


            container.Add(new Button(() => { DetectSamples(clipProperty, startSampleProperty); })
            {
                text = "Auto Detect Start Sample"
            });

            foldout.Add(container);
            return foldout;
        }

        private static void DetectSamples(SerializedProperty clipProperty, SerializedProperty startSampleProperty)
        {
            var audioClip = clipProperty.objectReferenceValue as AudioClip;
            if (audioClip == null)
            {
                Debug.LogError("No audio clip assigned");
                return;
            }

            var samples = AudioClipUtility.DetectSilenceSamples(audioClip);

            Undo.RecordObject(clipProperty.serializedObject.targetObject, "Detect Samples");

            startSampleProperty.intValue = samples.startSample;
            startSampleProperty.serializedObject.ApplyModifiedProperties();
        }

        private void AddSlider(VisualElement container, SerializedProperty property,
            string label, float min, float max)
        {
            var slider = new Slider(min, max)
            {
                showInputField = true,
                label = label
            };
            slider.BindProperty(property);
            container.Add(slider);
        }

        private void AddSliderFields(VisualElement container, SerializedProperty property)
        {
            var volumeProperty = property.FindPropertyRelative("_volume");
            var pitchProperty = property.FindPropertyRelative("_pitch");
            var priorityProperty = property.FindPropertyRelative("_priority");
            var panStereoProperty = property.FindPropertyRelative("_panStereo");

            var prioritySlider = new SliderInt(0, 256)
            {
                showInputField = true,
                label = "Priority"
            };
            prioritySlider.BindProperty(priorityProperty);
            container.Add(prioritySlider);

            AddSlider(container, volumeProperty, "Volume", 0, 1);
            AddSlider(container, pitchProperty, "Pitch", -3, 3);
            AddSlider(container, panStereoProperty, "Stereo Pan", -1, 1);
        }

        private void AddSamplesFields(VisualElement container, SerializedProperty property)
        {
            var startSampleProperty = property.FindPropertyRelative("_startSample");
            var loopPropertyField = property.FindPropertyRelative("_loop");

            container.Add(new PropertyField(loopPropertyField));
            container.Add(new PropertyField(startSampleProperty));
        }

        private void AddTimingFields(VisualElement container, SerializedProperty property)
        {
            var timingValueProperty = property.FindPropertyRelative("_delay");
            container.Add(new PropertyField(timingValueProperty));
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            EditorGUI.PropertyField(position, property, label, true);
            EditorGUI.EndProperty();
        }
    }
}