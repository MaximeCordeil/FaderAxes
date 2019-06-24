using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace VolumeRendering
{

    public class VolumeRenderingController : MonoBehaviour {

        [SerializeField] protected VolumeRendering volume;
        [SerializeField] protected Slider sliderXMin, sliderXMax, sliderYMin, sliderYMax, sliderZMin, sliderZMax;

        void Start ()
        {
            const float threshold = 0.025f;

            sliderXMin.onValueChanged.AddListener((v) =>
            {
                volume.sliceXMin = sliderXMin.value = Mathf.Min(v, volume.sliceXMax - threshold);
            });
            sliderXMax.onValueChanged.AddListener((v) =>
            {
                volume.sliceXMax = sliderXMax.value = Mathf.Max(v, volume.sliceXMin + threshold);
            });

            sliderYMin.onValueChanged.AddListener((v) =>
            {
                volume.sliceYMin = sliderYMin.value = Mathf.Min(v, volume.sliceYMax - threshold);
            });
            sliderYMax.onValueChanged.AddListener((v) =>
            {
                volume.sliceYMax = sliderYMax.value = Mathf.Max(v, volume.sliceYMin + threshold);
            });

            sliderZMin.onValueChanged.AddListener((v) =>
            {
                volume.sliceZMin = sliderZMin.value = Mathf.Min(v, volume.sliceZMax - threshold);
            });
            sliderZMax.onValueChanged.AddListener((v) =>
            {
                volume.sliceZMax = sliderZMax.value = Mathf.Max(v, volume.sliceZMin + threshold);
            });
        }

        float memoryR0 = 0f;
        float memoryR1 = 0f;

        public float rate = 1.5f;

        void Update()
        {
            //sliceXMin = Mathf.Min(sliderXMin.value, sliderXMax.value - threshold);

            //print(sliderXMin.value);

            //print(TEST.x0.ToString());

            
            

            volume.sliceXMin = 1f - TEST.x1 / 1023.0f;
            volume.sliceXMax = 1f - TEST.x0 / 1023.0f;
            volume.sliceYMin = TEST.y0 / 1023.0f;
            volume.sliceYMax = TEST.y1 / 1023.0f;
            volume.sliceZMin = TEST.z0 / 1023.0f;
            volume.sliceZMax = TEST.z1 / 1023.0f;

            //float rateR0 = 1f / Mathf.Abs(memoryR0 - TEST.r0) * rate;
            //float rateR1 = 1f / Mathf.Abs(memoryR1 - TEST.r1) * rate;


            //if (memoryR0 - TEST.r0 > 0f)
            //    volume.intensity += 0.05f / rateR0;
            //else if (memoryR0 - TEST.r0 < 0f)
            //    volume.intensity -= 0.05f / rateR0;

            ////if (volume.intensity < 0f) volume.intensity = 0f;
            ////if (volume.intensity > 1f) volume.intensity = 1f;

            //memoryR0 = TEST.r0;

            //if (memoryR1 - TEST.r1 > 0f)
            //    volume.threshold += 0.05f / rateR1;
            //else if (memoryR1 - TEST.r1 < 0f)
            //    volume.threshold -= 0.05f / rateR1;

            //if (volume.threshold < 0f) volume.threshold = 0f;
            //if (volume.threshold > 1f) volume.threshold = 1f;

            //memoryR1 = TEST.r1;

        }

        public void OnIntensity(float v)
        {
            volume.intensity = v;
        }

        public void OnThreshold(float v)
        {
            volume.threshold = v;
        }

    }

}


