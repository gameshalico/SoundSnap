using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace SoundSnap.Editor
{
    [CustomPropertyDrawer(typeof(SoundProfile))]
    public class SoundProfilePropertyDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var container = new VisualElement();

            var foldout = new Foldout { text = property.displayName };

            var clipProperty = property.FindPropertyRelative("_clip");
            var outputAudioMixerGroupProperty = property.FindPropertyRelative("_outputAudioMixerGroup");
            var muteProperty = property.FindPropertyRelative("_mute");
            var startSampleProperty = property.FindPropertyRelative("_startSample");
            var endSampleProperty = property.FindPropertyRelative("_endSample");
            var loopStartSampleProperty = property.FindPropertyRelative("_loopStartSample");

            var audioClipField = new PropertyField(clipProperty) { label = "AudioClip" };
            audioClipField.RegisterValueChangeCallback(evt =>
            {
                Undo.RecordObject(clipProperty.serializedObject.targetObject, "Change AudioClip");

                startSampleProperty.intValue = 0;
                loopStartSampleProperty.intValue = 0;
                endSampleProperty.intValue = -1;

                startSampleProperty.serializedObject.ApplyModifiedProperties();
            });
            container.Add(audioClipField);
            container.Add(new PropertyField(outputAudioMixerGroupProperty) { label = "Output" });
            container.Add(new PropertyField(muteProperty));

            AddSliderFields(container, property);
            AddSamplesFields(container, property);
            AddTimingFields(container, property);


            container.Add(new Button(() =>
            {
                DetectSamples(clipProperty, startSampleProperty, loopStartSampleProperty, endSampleProperty);
            })
            {
                text = "Auto Detect Start/End Sample"
            });

            foldout.Add(container);
            return foldout;
        }

        private static void DetectSamples(SerializedProperty clipProperty, SerializedProperty startSampleProperty,
            SerializedProperty loopStartSampleProperty, SerializedProperty endSampleProperty)
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
            loopStartSampleProperty.intValue = samples.startSample;
            endSampleProperty.intValue = samples.endSample;

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
            var endSampleProperty = property.FindPropertyRelative("_endSample");

            var loopStartSampleProperty = property.FindPropertyRelative("_loopStartSample");
            var loopCountProperty = property.FindPropertyRelative("_loopCount");
            var isLoopIntervalPreservedProperty = property.FindPropertyRelative("_isLoopIntervalPreserved");

            container.Add(new PropertyField(loopStartSampleProperty));
            container.Add(new PropertyField(loopCountProperty));
            container.Add(new PropertyField(isLoopIntervalPreservedProperty));

            container.Add(new PropertyField(startSampleProperty));
            container.Add(new PropertyField(endSampleProperty));
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