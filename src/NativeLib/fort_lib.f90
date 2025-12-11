module coord_type_mod
    use iso_c_binding
    implicit none

    ! BIND(C) ensures binary compatibility with the C struct
    type, bind(c) :: coordinate_t
        integer(c_int) :: x
        integer(c_int) :: y
    end type coordinate_t

    type, bind(c) :: distance_t
        integer(c_int) :: i
        integer(c_int) :: j
        integer(c_int64_t) :: dist
    end type distance_t

end module

subroutine rectangle_areas(i_ptr, o_ptr, num_elements) bind(c, name="rectangle_areas")
  use iso_c_binding
  use coord_type_mod
  implicit none

  type(c_ptr), value, intent(in) :: i_ptr, o_ptr
  integer(c_int), value, intent(in) :: num_elements
  type(coordinate_t), pointer :: i_array(:)
  type(distance_t), pointer :: o_array(:)
  integer :: i, j, combs, flat_index

  combs = (int(num_elements) * (int(num_elements) - 1)) / 2

  call c_f_pointer(i_ptr, i_array, [num_elements])
  call c_f_pointer(o_ptr, o_array, [combs])

  do i = 1, num_elements - 1
    do j = i + 1, num_elements
      flat_index = (i-1)*num_elements + j - i*(i+1)/2 
      o_array(flat_index)%dist = &
      int((abs(i_array(i)%x - i_array(j)%x) + 1), 8) * &
      int((abs(i_array(i)%y - i_array(j)%y) + 1), 8)
      o_array(flat_index)%i = i - 1
      o_array(flat_index)%j = j - 1
    end do
  end do

end subroutine rectangle_areas