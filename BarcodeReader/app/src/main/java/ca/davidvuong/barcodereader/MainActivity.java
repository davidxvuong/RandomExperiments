package ca.davidvuong.barcodereader;

import android.content.Intent;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.view.Menu;
import android.view.MenuItem;
import android.view.View;
import android.widget.Button;
import android.widget.TextView;
import android.widget.Toast;

import com.google.zxing.integration.android.*;

public class MainActivity extends AppCompatActivity implements View.OnClickListener {
    private Button btnScan;
    private TextView formatTxt, contentTxt;


    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        btnScan = (Button) findViewById(R.id.btnScan);
        formatTxt = (TextView) findViewById(R.id.scan_format);
        contentTxt = (TextView) findViewById(R.id.scan_content);

        btnScan.setOnClickListener(this);
    }

    @Override
    public void onClick(View v) {
        if(v.getId()==R.id.btnScan) {
            //Initiate scan
            IntentIntegrator scanIntegrator = new IntentIntegrator(this);

            scanIntegrator.initiateScan();

        }
    }

    public void onActivityResult(int requestCode, int resultCode, Intent intent) {
        //get scan result
        IntentResult getResult = IntentIntegrator.parseActivityResult(requestCode, resultCode, intent);

        if (getResult != null) {
            String scanContent = getResult.getContents();
            String scanFormat = getResult.getFormatName();

            formatTxt.setText(String.format("FORMAT: %1$s", scanFormat));
            contentTxt.setText(String.format("CONTENT: %1$s", scanContent));
        }
        else {
            Toast.makeText(this, "No data!", Toast.LENGTH_LONG).show();
        }
    }
}
