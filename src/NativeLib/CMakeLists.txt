cmake_minimum_required(VERSION 3.7)

project(fort_lib LANGUAGES Fortran CXX)

set(HIGHS_DIR /home/linuxbrew/.linuxbrew/Cellar/highs/1.12.0/lib/cmake)

find_package(HIGHS REQUIRED)

add_library(fort_lib SHARED fort_lib.f90)

add_library(highs_cpp SHARED highs_cpp.cpp)
target_link_libraries(highs_cpp highs::highs)