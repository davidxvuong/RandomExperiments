package ca.davidvuong.myapplication;

import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.view.Menu;
import android.view.MenuItem;
import android.view.View;
import android.widget.Button;
import android.widget.TextView;

public class MainActivity extends AppCompatActivity {

    Button button;
    TextView text;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        button = (Button)(findViewById(R.id.button));
        text = (TextView)(findViewById(R.id.textView));

        button.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {

                text.append("Theresa is awesome!!");
            }
        });
    }


}
