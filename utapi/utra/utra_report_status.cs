using System;
using utapi.basic;
using utapi.common;

namespace utapi.utra
{
    class UtraReportStatus10Hz : ArmReportStatus
    {
        // """This class will create a new thread to connect to ubot and receive the status of ubot at a frequency of 10HZ
        // The status is as follows:
        //     axis (int): number of arm axes
        //     motion_status (int): running status of the arm
        //     motion_mode (int): operating mode of the arm
        //     mt_brake (int): enable state of the joint brake
        //     mt_able (int): enable state of the arm
        //     err_code (int): error code
        //     war_code (int): warning code
        //     cmd_num (int): current number of instruction cache
        //     joint (list): the actual angular positions of all joints
        //     pose (list):the current measured tool pose
        //     tau (list):the actual angular current of all joints
        // Args:
        //     ip (String): IP address of UTRA robotic arm
        // """
        public UtraReportStatus10Hz(String ip)
        {
            this.__init__(ip, 30001);
        }
    }


    class UtraReportStatus100Hz : ArmReportStatus
    {
        //  """This class will create a new thread to connect to ubot and receive the status of ubot at a frequency of 100HZ
        // The status is as follows:
        //     see the UtraReportStatus10HZ

        // Args:
        //     ip (String): IP address of UTRA robotic arm
        // """
        public UtraReportStatus100Hz(String ip)
        {
            this.__init__(ip, 30002);
        }
    }
   
}
