using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace iPlant.FMS.Models
{
    /// <summary>
    /// �Ͽ�
    /// </summary>
    public class MSSMaterialFrame : BasePo
    {

        public double FrameHeight { get; set; } = 0.0;

        public double SteelPlateHeight { get; set; } = 0.0;

        public double SteelPlateNum { get; set; } = 0.0;

        public String FrameCode { get; set; } = "";

        public bool IsExistFrame { get; set; } = false;

        public DateTime ArrivalTime { get; set; } = new DateTime(2000, 1, 1);

        //�������
        public string OrderNo { get; set; } = "";
        //���ϱ���
        public string MaterialNo { get; set; } = "";
        //��Ʒ���
        public string PartID { get; set; } = "";
        //��Ʒ����
        public string PartType { get; set; } = "";

        //�Ͽ�״̬
        // 1:  ���Ͽ� 
        // 2�����Ͽ�������Ͽ�
        // 3�����Ͽ���������
        public int FrameStatus { get; set; } = 0;
        //����״̬
        // 0:  �޺��� 
        // 1����������
        // 2��������Ͽ�
        public int CallStatus { get; set; } = 0;
        public string WorkCenter { get; set; } = "";


        /// <summary>
        /// ˳���
        /// </summary>
        public string SourceSequenceNo { get; set; } = "";
        /// <summary>
        /// �����
        /// </summary>
        public string SourceOprSequenceNo { get; set; } = "";
    }
}