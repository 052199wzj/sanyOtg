package com.mes.server.service.po.dms;

import java.io.Serializable;


import com.mes.server.service.utils.StringUtils;

/// <summary>
/// 耗损倍率
/// </summary>
public class DMSExpenditureRatio implements Serializable {
	/**
	 * 
	 */
	private static final long serialVersionUID = 1L;
	public int LeftTimes;
	public int RightTimes;
	public double Ratio;

	public DMSExpenditureRatio() {
 
	}

	@Override
	public String ToString() {
		return StringUtils.Format("{0}+|,|+{1}+|,|+{2}", String.valueOf(LeftTimes), String.valueOf(RightTimes),
				String.valueOf(Ratio));
	}

	public DMSExpenditureRatio(String wFormatText) {
		if (wFormatText == null)
			return;
		String[] wStringList = wFormatText.split("\\+\\|,\\|\\+");

		if (wStringList.length != 3)
			return;

		LeftTimes = StringUtils.parseInt(wStringList[0]);
		RightTimes = StringUtils.parseInt(wStringList[1]);
		Ratio = StringUtils.parseDouble(wStringList[2]);

	}

	public int getLeftTimes() {
		return LeftTimes;
	}

	public void setLeftTimes(int leftTimes) {
		LeftTimes = leftTimes;
	}

	public int getRightTimes() {
		return RightTimes;
	}

	public void setRightTimes(int rightTimes) {
		RightTimes = rightTimes;
	}

	public double getRatio() {
		return Ratio;
	}

	public void setRatio(double ratio) {
		Ratio = ratio;
	}
}
