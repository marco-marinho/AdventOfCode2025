#include <vector>
#include <iostream>
#include <cassert>
#include <vector>
#include "Highs.h"

extern "C"
{
    double mip_solve(
        const double *const c,
        const double *const A_flat,
        const double *const b,
        int num_rows,
        int num_cols)
    {
        Highs highs;
        HighsStatus return_status;

        highs.setOptionValue("log_to_console", false);
        highs.setOptionValue("output_flag", false);

        std::vector<int> a_start;
        std::vector<int> a_index;
        std::vector<double> a_value;

        a_start.push_back(0); // Start of column 0

        for (int col = 0; col < num_cols; ++col)
        {
            for (int row = 0; row < num_rows; ++row)
            {
                // Access row-major data: A[row, col]
                double val = A_flat[row * num_cols + col];
                if (val > 1e-9){
                    a_index.push_back(row);
                    a_value.push_back(val);
                }
            }
            // Mark where the NEXT column begins
            a_start.push_back(static_cast<int>(a_value.size()));
        }

        int num_nz = a_value.size();

        // For Ax = b, rows must be between b and b
        std::vector<double> row_lower = std::vector<double>(b, b + num_rows);
        std::vector<double> row_upper = std::vector<double>(b, b + num_rows);
        ;

        // For x >= 0, cols must be between 0 and Infinity
        std::vector<double> col_lower(num_cols, 0.0);
        std::vector<double> col_upper(num_cols, kHighsInf);

        HighsModel model;
        model.lp_.num_col_ = num_cols;
        model.lp_.num_row_ = num_rows;
        model.lp_.sense_ = ObjSense::kMinimize;
        model.lp_.offset_ = 0.0;
        model.lp_.col_cost_ = std::vector<double>(c, c + num_cols);
        model.lp_.col_lower_ = col_lower;
        model.lp_.col_upper_ = col_upper;
        model.lp_.row_lower_ = row_lower;
        model.lp_.row_upper_ = row_upper;

        model.lp_.integrality_ = std::vector<HighsVarType>(num_cols, HighsVarType::kInteger);

        model.lp_.a_matrix_.format_ = MatrixFormat::kColwise;
        model.lp_.a_matrix_.start_ = a_start;
        model.lp_.a_matrix_.index_ = a_index;
        model.lp_.a_matrix_.value_ = a_value;

        return_status = highs.passModel(model);
        if(return_status != HighsStatus::kOk){
            return -1.0;
        }

        return_status = highs.run();
        if(return_status != HighsStatus::kOk){
            return -1.0;
        }

        const HighsModelStatus &model_status = highs.getModelStatus();
        const HighsInfo &info = highs.getInfo();
        if (model_status == HighsModelStatus::kOptimal)
        {
            return info.objective_function_value;
        }
        else
        {
            return -1.0;
        }
    }
}
