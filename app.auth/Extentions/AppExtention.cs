namespace app.auth.Extentions;
public static class AppExtention
{
    public static void UseArchtectures(this WebApplication app)
    {
        app.UseAuthentication();
        app.UseAuthorization();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                //options.RoutePrefix = string.Empty; // Set Swagger UI at the app's root
            });
        }
    }
}