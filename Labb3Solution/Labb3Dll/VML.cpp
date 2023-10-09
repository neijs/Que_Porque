#include "mkl.h"
#include "pch.h"
#include <cmath>
#include <chrono>

extern "C" __declspec(dllexport) void TanRequest(int size, double* src, double* vdTan, double* tan, long long* vdTanTime, long long* tanTime);

void TanRequest(int size, double* src, double* VdTanRes, double* TanRes, long long* vdTanTime, long long* tanTime) {
	auto start = std::chrono::high_resolution_clock::now();
	for (int i = 0; i < size; ++i) {
		TanRes[i] = tan(src[i]);
	}
	auto stop = std::chrono::high_resolution_clock::now();
	auto duration = std::chrono::duration_cast<std::chrono::microseconds>(stop - start);
	*tanTime = duration.count();

	auto start1 = std::chrono::high_resolution_clock::now();
	vdTan(size, src, VdTanRes);
	auto stop1 = std::chrono::high_resolution_clock::now();
	auto duration1 = std::chrono::duration_cast<std::chrono::microseconds>(stop1 - start1);
	*vdTanTime = duration1.count();
}
