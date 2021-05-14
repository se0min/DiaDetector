using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutoAssembler.Data
{
    public struct CameraSettingInfo
    {
        public string Name;
        public string IP;
        public int ScreenSize;
        public double FrameRate;
        public int ZoomFocusPort;   // 2016-05-27 - Zoom & Focus 포트가 공용으므로 리팩토링으로 변수를 통합함.
        public int FocusPort;       // 2016-05-27 - Zoom & Focus 포트가 공용으므로 사용하지 않게 됨.
    }
}
