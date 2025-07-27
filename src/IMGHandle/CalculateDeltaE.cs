using sharp_render.src.Common;

namespace sharp_render.src.IMGHandle
{
    public static class CieDe2000Comparison
    {
        private const float Epsilon = 0.008856F;
        private const float Kappa = 903.3F;
        private const float WhiteX = 95.047F;
        private const float WhiteY = 100.000F;
        private const float WhiteZ = 108.883F;

        public static float CalculateDeltaE(Color c1, Color c2)
        {
            float r1 = PivotRgb(c1.R / 255.0F);
            float g1 = PivotRgb(c1.G / 255.0F);
            float b1 = PivotRgb(c1.B / 255.0F);
            float x1 = r1 * 0.4124F + g1 * 0.3576F + b1 * 0.1805F;
            float y1 = r1 * 0.2126F + g1 * 0.7152F + b1 * 0.0722F;
            float z1 = r1 * 0.0193F + g1 * 0.1192F + b1 * 0.9505F;
            float r2 = PivotRgb(c2.R / 255.0F);
            float g2 = PivotRgb(c2.G / 255.0F);
            float b2 = PivotRgb(c2.B / 255.0F);
            float x2 = r2 * 0.4124F + g2 * 0.3576F + b2 * 0.1805F;
            float y2 = r2 * 0.2126F + g2 * 0.7152F + b2 * 0.0722F;
            float z2 = r2 * 0.0193F + g2 * 0.1192F + b2 * 0.9505F;
            float fx1 = PivotXyz(x1 / WhiteX);
            float fy1 = PivotXyz(y1 / WhiteY);
            float fz1 = PivotXyz(z1 / WhiteZ);
            float L1 = Math.Max(0, 116 * fy1 - 16);
            float a1 = 500 * (fx1 - fy1);
            float b1_lab = 200 * (fy1 - fz1);
            float fx2 = PivotXyz(x2 / WhiteX);
            float fy2 = PivotXyz(y2 / WhiteY);
            float fz2 = PivotXyz(z2 / WhiteZ);
            float L2 = Math.Max(0, 116 * fy2 - 16);
            float a2 = 500 * (fx2 - fy2);
            float b2_lab = 200 * (fy2 - fz2);
            float c_star_1_ab = (float)Math.Sqrt(a1 * a1 + b1_lab * b1_lab);
            float c_star_2_ab = (float)Math.Sqrt(a2 * a2 + b2_lab * b2_lab);
            float c_star_average_ab = (c_star_1_ab + c_star_2_ab) * 0.5F;
            float c_star_average_ab_pot7 = (float)Math.Pow(c_star_average_ab, 7F);
            float G = (float)(
                0.5F
                * (1 - Math.Sqrt(c_star_average_ab_pot7 / (c_star_average_ab_pot7 + 6103515625)))
            );
            float a1_prime = (1 + G) * a1;
            float a2_prime = (1 + G) * a2;
            float C_prime_1 = (float)Math.Sqrt(a1_prime * a1_prime + b1_lab * b1_lab);
            float C_prime_2 = (float)Math.Sqrt(a2_prime * a2_prime + b2_lab * b2_lab);
            float h_prime_1 = (float)(
                ((Math.Atan2(b1_lab, a1_prime) * 180.0F / Math.PI) + 360) % 360.0F
            );
            float h_prime_2 = (float)(
                ((Math.Atan2(b2_lab, a2_prime) * 180.0F / Math.PI) + 360) % 360.0F
            );
            float delta_L_prime = L2 - L1;
            float delta_C_prime = C_prime_2 - C_prime_1;
            float h_bar = Math.Abs(h_prime_1 - h_prime_2);
            float has_chroma = (float)Math.Min(1.0, Math.Abs(C_prime_1 * C_prime_2) * 1000.0F);
            float h_diff = h_prime_2 - h_prime_1;
            float use_direct = SmoothStep(180.0F, h_bar);
            float use_plus360 = (1.0F - use_direct) * SmoothStep(h_prime_2, h_prime_1);
            float use_minus360 = (1.0F - use_direct) * (1.0F - SmoothStep(h_prime_2, h_prime_1));
            float delta_h_prime =
                has_chroma
                * (
                    h_diff * use_direct
                    + (h_diff + 360.0F) * use_plus360
                    + (h_diff - 360.0F) * use_minus360
                );
            float delta_H_prime = (float)(
                2 * Math.Sqrt(C_prime_1 * C_prime_2) * Math.Sin(delta_h_prime * Math.PI / 360.0F)
            );
            float h_sum = h_prime_1 + h_prime_2;
            float use_sum_direct = SmoothStep(180.0F, h_bar);
            float use_sum_plus360 = (1.0F - use_sum_direct) * SmoothStep(360.0F, h_sum);
            float use_sum_minus360 = (1.0F - use_sum_direct) * (1.0F - SmoothStep(360.0F, h_sum));
            float h_prime_average =
                has_chroma
                * 0.5F
                * (
                    h_sum * use_sum_direct
                    + (h_sum + 360.0F) * use_sum_plus360
                    + (h_sum - 360.0F) * use_sum_minus360
                );
            float L_prime_average = (L1 + L2) * 0.5F;
            float C_prime_average = (C_prime_1 + C_prime_2) * 0.5F;
            float L_prime_average_minus_50_square = (L_prime_average - 50) * (L_prime_average - 50);
            float S_L = (float)(
                1
                + (
                    0.015F
                    * L_prime_average_minus_50_square
                    / Math.Sqrt(20 + L_prime_average_minus_50_square)
                )
            );
            float S_C = (float)(1 + 0.045 * C_prime_average);
            float h_rad = (float)(h_prime_average * Math.PI / 180.0F);
            float T = (float)(
                1
                - 0.17 * Math.Cos(h_rad - 30.0F * Math.PI / 180.0F)
                + 0.24 * Math.Cos(2 * h_rad)
                + 0.32 * Math.Cos(3 * h_rad + 6.0F * Math.PI / 180.0F)
                - 0.2 * Math.Cos(4 * h_rad - 63.0F * Math.PI / 180.0F)
            );
            float S_H = (float)(1 + 0.015 * T * C_prime_average);
            float h_prime_average_minus_275_div_25_square = (float)((h_prime_average - 275) / 25.0);
            h_prime_average_minus_275_div_25_square *= h_prime_average_minus_275_div_25_square;
            float delta_theta = (float)(30 * Math.Exp(-h_prime_average_minus_275_div_25_square));
            float C_prime_average_pot_7 = (float)Math.Pow(C_prime_average, 7);
            float R_C = (float)(
                2 * Math.Sqrt(C_prime_average_pot_7 / (C_prime_average_pot_7 + 6103515625))
            );
            float R_T = (float)(-Math.Sin(2 * delta_theta * Math.PI / 180.0F) * R_C);
            float delta_L_prime_div_k_L_S_L = delta_L_prime / S_L;
            float delta_C_prime_div_k_C_S_C = delta_C_prime / S_C;
            float delta_H_prime_div_k_H_S_H = delta_H_prime / S_H;
            return (float)
                Math.Sqrt(
                    delta_L_prime_div_k_L_S_L * delta_L_prime_div_k_L_S_L
                        + delta_C_prime_div_k_C_S_C * delta_C_prime_div_k_C_S_C
                        + delta_H_prime_div_k_H_S_H * delta_H_prime_div_k_H_S_H
                        + R_T * delta_C_prime_div_k_C_S_C * delta_H_prime_div_k_H_S_H
                );
        }

        private static float PivotRgb(float n)
        {
            float t = (float)(1.0F / (1.0F + Math.Exp(-150.0F * (n - 0.04045F))));
            float linear = n / 12.92F;
            float gamma = (float)Math.Pow((n + 0.055F) / 1.055F, 2.4F);
            return (linear * (1.0F - t) + gamma * t) * 100.0F;
        }

        private static float PivotXyz(float n)
        {
            float t = (float)(1.0F / (1.0F + Math.Exp(-500.0F * (n - Epsilon))));
            float linear = (Kappa * n + 16) / 116;
            float cubic = (float)Math.Pow(n, 1.0F / 3.0F);
            return linear * (1.0F - t) + cubic * t;
        }

        private static float SmoothStep(float edge, float x, float smoothness = 100.0F)
        {
            return (float)(1.0F / (1.0F + Math.Exp(-smoothness * (x - edge))));
        }
    }
}
