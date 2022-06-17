namespace utapi.basic
{
    class SERVO_REG
    {
        private static byte Null = 0;

        //# cmd的reg  读reg发送cmd的长度  读reg接收data的长度  写reg发送cmd的长度  写reg接收data的长度
        public static byte[] UUID = new byte[] { 0x01, 0, 12, Null, Null };

        public static byte[] SW_VERSION = new byte[] { 0x02, 0, 12, Null, Null };

        public static byte[] HW_VERSION = new byte[] { 0x03, 0, 12, Null, Null };

        public static byte[] MULTI_VERSION = new byte[] { 0x04, 0, 12, Null, Null };

        public static byte[] MECH_RATIO = new byte[] { 0x05, 0, 4, 4, 0 };

        public static byte[] COM_ID = new byte[] { 0x08, Null, Null, 1, 0 };

        public static byte[] COM_BAUD = new byte[] { 0x09, Null, Null, 4, 0 };

        public static byte[] RESET_ERR = new byte[] { 0x0C, Null, Null, 1, 0 };

        public static byte[] REBOOT_DRIVER = new byte[] { 0x0D, Null, Null, 1, 0 };

        public static byte[] ERASE_PARM = new byte[] { 0x0E, Null, Null, 1, 0 };

        public static byte[] SAVED_PARM = new byte[] { 0x0F, Null, Null, 1, 0 };

        public static byte[] ELEC_RATIO = new byte[] { 0x10, 0, 4, 4, 0 };

        public static byte[] MOTION_DIR = new byte[] { 0x11, 0, 1, 1, 0 };

        public static byte[] IWDG_CYC = { 0x12, 0, 4, 4, 0 };

        public static byte[] TEMP_LIMIT = new byte[] { 0x18, 0, 2, 2, 0 };

        public static byte[] VOLT_LIMIT = new byte[] { 0x19, 0, 2, 2, 0 };

        public static byte[] CURR_LIMIT = new byte[] { 0x1A, 0, 4, 4, 0 };

        public static byte[] BRAKE_PWM = new byte[] { 0x1F, 0, 1, 1, 0 };

        public static byte[] MOTION_MDOE = new byte[] { 0x20, 0, 1, 1, 0 };

        public static byte[] MOTION_ENABLE = new byte[] { 0x21, 0, 1, 1, 0 };

        public static byte[] BRAKE_ENABLE = new byte[] { 0x22, 0, 1, 1, 0 };

        public static byte[] TEMP_DRIVER = new byte[] { 0x28, 0, 4, Null, Null };

        public static byte[] TEMP_MOTOR = new byte[] { 0x29, 0, 4, Null, Null };

        public static byte[] BUS_VOLT = new byte[] { 0x2A, 0, 4, Null, Null };

        public static byte[] BUS_CURR = new byte[] { 0x2B, 0, 4, Null, Null };

        public static byte[] MULTI_VOLT = new byte[] { 0x2C, 0, 4, Null, Null };

        public static byte[] ERROR_CODE = new byte[] { 0x2F, 0, 1, Null, Null };

        public static byte[] POS_TARGET = new byte[] { 0x30, 0, 4, 4, 0 };

        public static byte[] POS_CURRENT = new byte[] { 0x31, 0, 4, Null, Null };

        public static byte[] POS_LIMIT_MAX = new byte[] { 0x32, 0, 4, 4, 0 };

        public static byte[] POS_LIMIT_MIN = new byte[] { 0x33, 0, 4, 4, 0 };

        public static byte[] POS_LIMIT_DIFF = new byte[] { 0x34, 0, 4, 4, 0 };

        public static byte[] POS_PIDP = new byte[] { 0x35, 0, 4, 4, 0 };

        public static byte[] POS_SMOOTH_CYC = new byte[] { 0x36, 0, 1, 1, 0 };

        public static byte[] POS_ADRC_PARAM = new byte[] { 0x39, 1, 4, 5, 0 };

        public static byte[] POS_CAL_ZERO = new byte[] { 0x3F, Null, Null, 1, 0 };

        public static byte[] VEL_TARGET = new byte[] { 0x40, 0, 4, 4, 0 };

        public static byte[] VEL_CURRENT = new byte[] { 0x41, 0, 4, Null, Null };

        public static byte[] VEL_LIMIT_MAX = new byte[] { 0x42, 0, 4, 4, 0 };

        public static byte[] VEL_LIMIT_MIN = new byte[] { 0x43, 0, 4, 4, 0 };

        public static byte[] VEL_LIMIT_DIFF = new byte[] { 0x44, 0, 4, 4, 0 };

        public static byte[] VEL_PIDP = new byte[] { 0x45, 0, 4, 4, 0 };

        public static byte[] VEL_PIDI = new byte[] { 0x46, 0, 4, 4, 0 };

        public static byte[] VEL_SMOOTH_CYC = new byte[] { 0x47, 0, 1, 1, 0 };
        
        public static byte[] VEL_ADRC_PARAM = new byte[] { 0x49, 1, 4, 5, 0 };

        public static byte[] TAU_TARGET = new byte[] { 0x50, 0, 4, 4, 0 };

        public static byte[] TAU_CURRENT = new byte[] { 0x51, 0, 4, Null, Null };

        public static byte[] TAU_LIMIT_MAX = new byte[] { 0x52, 0, 4, 4, 0 };

        public static byte[] TAU_LIMIT_MIN = new byte[] { 0x53, 0, 4, 4, 0 };

        public static byte[] TAU_LIMIT_DIFF = new byte[] { 0x54, 0, 4, 4, 0 };

        public static byte[] TAU_PIDP = new byte[] { 0x55, 0, 4, 4, 0 };

        public static byte[] TAU_PIDI = new byte[] { 0x56, 0, 4, 4, 0 };
        
        public static byte[] TAU_ADRC_PARAM = new byte[] { 0x59, 1, 4, 5, 0 };

        public static byte[] TAU_SMOOTH_CYC = new byte[] { 0x57, 0, 1, 1, 0 };

        
    }
}
