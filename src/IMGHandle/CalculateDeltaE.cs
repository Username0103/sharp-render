using sharp_render.src.Common;

namespace sharp_render.src.IMGHandle
{
    public static class CieDe2000Comparison
    {
        private const double Epsilon = 0.008856;
        private const double Kappa = 903.3;
        private const double WhiteX = 95.047;
        private const double WhiteY = 100.000;

        public static double CalculateDeltaE(Color c1, Color c2)
        {
            double r1 = PivotRgb(c1.R / 255.0);
            double g1 = PivotRgb(c1.G / 255.0);
            double b1 = PivotRgb(c1.B / 255.0);
            double x1 = r1 * 0.4124 + g1 * 0.3576 + b1 * 0.1805;
            double y1 = r1 * 0.2126 + g1 * 0.7152 + b1 * 0.0722;
            double r2 = PivotRgb(c2.R / 255.0);
            double g2 = PivotRgb(c2.G / 255.0);
            double b2 = PivotRgb(c2.B / 255.0);
            double x2 = r2 * 0.4124 + g2 * 0.3576 + b2 * 0.1805;
            double y2 = r2 * 0.2126 + g2 * 0.7152 + b2 * 0.0722;
            double fx1 = PivotXyz(x1 / WhiteX);
            double fy1 = PivotXyz(y1 / WhiteY);
            double L1 = Math.Max(0, 116 * fy1 - 16);
            double a1 = 500 * (fx1 - fy1);
            double fx2 = PivotXyz(x2 / WhiteX);
            double fy2 = PivotXyz(y2 / WhiteY);
            double L2 = Math.Max(0, 116 * fy2 - 16);
            double a2 = 500 * (fx2 - fy2);
            double c_star_1_ab = Math.Sqrt(a1 * a1 + b1 * b1);
            double c_star_2_ab = Math.Sqrt(a2 * a2 + b2 * b2);
            double c_star_average_ab = (c_star_1_ab + c_star_2_ab) * 0.5;
            double c_star_average_ab_pot7 = c_star_average_ab * c_star_average_ab * c_star_average_ab;
            c_star_average_ab_pot7 *= c_star_average_ab_pot7 * c_star_average_ab;
            double G = 0.5 * (1 - Math.Sqrt(c_star_average_ab_pot7 / (c_star_average_ab_pot7 + 6103515625)));
            double a1_prime = (1 + G) * a1;
            double a2_prime = (1 + G) * a2;
            double C_prime_1 = Math.Sqrt(a1_prime * a1_prime + b1 * b1);
            double C_prime_2 = Math.Sqrt(a2_prime * a2_prime + b2 * b2);
            double h_prime_1 = ((Math.Atan2(b1, a1_prime) * 180.0 / Math.PI) + 360) % 360.0;
            double h_prime_2 = ((Math.Atan2(b2, a2_prime) * 180.0 / Math.PI) + 360) % 360.0;
            double delta_L_prime = L2 - L1;
            double delta_C_prime = C_prime_2 - C_prime_1;
            double h_bar = Math.Abs(h_prime_1 - h_prime_2);
            double delta_h_prime;
            if (C_prime_1 * C_prime_2 == 0)
            {
                delta_h_prime = 0;
            }
            else if (h_bar <= 180.0)
            {
                delta_h_prime = h_prime_2 - h_prime_1;
            }
            else if (h_prime_2 <= h_prime_1)
            {
                delta_h_prime = h_prime_2 - h_prime_1 + 360.0;
            }
            else
            {
                delta_h_prime = h_prime_2 - h_prime_1 - 360.0;
            }
            double delta_H_prime = 2 * Math.Sqrt(C_prime_1 * C_prime_2) * Math.Sin(delta_h_prime * Math.PI / 360.0);

            double L_prime_average = (L1 + L2) * 0.5;
            double C_prime_average = (C_prime_1 + C_prime_2) * 0.5;
            double h_prime_average;
            if (C_prime_1 * C_prime_2 == 0)
            {
                h_prime_average = 0;
            }
            else if (h_bar <= 180.0)
            {
                h_prime_average = (h_prime_1 + h_prime_2) * 0.5;
            }
            else if ((h_prime_1 + h_prime_2) < 360.0)
            {
                h_prime_average = (h_prime_1 + h_prime_2 + 360.0) * 0.5;
            }
            else
            {
                h_prime_average = (h_prime_1 + h_prime_2 - 360.0) * 0.5;
            }
            double L_prime_average_minus_50_square = (L_prime_average - 50) * (L_prime_average - 50);

            double S_L = 1 + (0.015 * L_prime_average_minus_50_square / Math.Sqrt(20 + L_prime_average_minus_50_square));
            double S_C = 1 + 0.045 * C_prime_average;
            double h_rad = h_prime_average * Math.PI / 180.0;
            double T = 1
                - 0.17 * Math.Cos(h_rad - 30.0 * Math.PI / 180.0)
                + 0.24 * Math.Cos(2 * h_rad)
                + 0.32 * Math.Cos(3 * h_rad + 6.0 * Math.PI / 180.0)
                - 0.2 * Math.Cos(4 * h_rad - 63.0 * Math.PI / 180.0);
            double S_H = 1 + 0.015 * T * C_prime_average;
            double h_prime_average_minus_275_div_25_square = (h_prime_average - 275) / 25.0;
            h_prime_average_minus_275_div_25_square *= h_prime_average_minus_275_div_25_square;
            double delta_theta = 30 * Math.Exp(-h_prime_average_minus_275_div_25_square);
            double C_prime_average_pot_7 = C_prime_average * C_prime_average * C_prime_average;
            C_prime_average_pot_7 *= C_prime_average_pot_7 * C_prime_average;
            double R_C = 2 * Math.Sqrt(C_prime_average_pot_7 / (C_prime_average_pot_7 + 6103515625));
            double R_T = -Math.Sin(2 * delta_theta * Math.PI / 180.0) * R_C;
            double delta_L_prime_div_k_L_S_L = delta_L_prime / S_L;
            double delta_C_prime_div_k_C_S_C = delta_C_prime / S_C;
            double delta_H_prime_div_k_H_S_H = delta_H_prime / S_H;
            return Math.Sqrt(
                delta_L_prime_div_k_L_S_L * delta_L_prime_div_k_L_S_L
                + delta_C_prime_div_k_C_S_C * delta_C_prime_div_k_C_S_C
                + delta_H_prime_div_k_H_S_H * delta_H_prime_div_k_H_S_H
                + R_T * delta_C_prime_div_k_C_S_C * delta_H_prime_div_k_H_S_H
            );
        }

        private static double PivotRgb(double n)
        {
            return (n > 0.04045 ? Math.Pow((n + 0.055) / 1.055, 2.4) : n / 12.92) * 100.0;
        }

        private static double PivotXyz(double n)
        {
            return n > Epsilon ? Math.Pow(n, 1.0 / 3.0) : (Kappa * n + 16) / 116;
        }
    }
}
