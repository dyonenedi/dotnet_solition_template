namespace app.auth.Extentions;
public static class AppExtention
{
    public static void UseArchtectures(this WebApplication app)
    {
        app.UseAuthentication();
        app.UseAuthorization();
        if (app.Environment.IsDevelopment())
        {
            //app.MapOpenApi();
            app.UseSwagger();
        }
        app.UseHttpsRedirection();
    }
}