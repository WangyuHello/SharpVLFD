//------------------------------------------------
// FILE: AlarmClock.v
// AUTHOR: Lora Chang
// $Id$
// ABSTRATE: A clock with alarm
// KEYWORDS: alarm, counter
// MODIFICATION HISTORY:
// $Log$
//      Lora    10/10/04  Origial
//      Lora    03/27/05  When set_hr/set_min is active, the clock continues.
// (c) copyright 2005 SMIMS Inc. All rights reserved
//------------------------------------------------

module AlarmClock(clk, rst_n, mode, set_hr, set_min, hr_out, min_out, sec_out, 
                  hr_alarm, min_alarm, alarm); 

input         clk;                   // System clock
input         rst_n;                 // Asyn. reset, low active
input  		    mode;                  // Input Mode Selection; 
                                     //  1 : set clock; 
                                     //  0 : set alarm
input   	    set_hr;                // Set hour input signal
                                     //  Active : when 0 -> 1
                                     //  when mode = 1, set_hr actives -> set clock hour
                                     //  when mode = 0, set_hr actives -> set Alarm hour
input   	    set_min;               // Set minute input signal
                                     //  Active : when 0 -> 1
output 	[3:0]	hr_out;                // Clock hour output
output 	[5:0]	min_out;               // Clock minute output
output	[5:0]	sec_out;               // Clock second output
output 	[3:0]	hr_alarm;              // Alarm hour output
output 	[5:0]	min_alarm;             // Alarm minute output
output		    alarm;                 // when time of clock is equal to time of alarm, alarm actives

// ------------
// Local Signal
// ------------   
   reg 	[3:0]	hr_out;
   reg 	[5:0]	min_out;
   reg	[5:0]	sec_out;
   reg		    alarm;
   wire 	    set_clock;
   wire 	    set_alarm;
   wire 	    hr_pulse;
   wire		    min_pulse;
   wire 	    sec_pulse;
   reg		    set_hr_d;
   reg		    set_min_d;
   reg 	[3:0]	hr_alarm;
   reg 	[5:0]	min_alarm;

// ------------
// Main Circuit
// ------------

   assign hr_pulse = ~set_hr_d & set_hr;
   assign min_pulse = ~set_min_d & set_min;
   assign set_alarm = (mode) & (hr_pulse | min_pulse);
   
   always @(posedge clk or negedge rst_n)
       if (~rst_n) begin
       	   set_hr_d <= 0;
       	   set_min_d <= 0;
       end else begin
       	   set_hr_d <= set_hr;
       	   set_min_d <= set_min;
       end


   always @(posedge clk or negedge rst_n)
       if (~rst_n) begin
       	   hr_out <= 12;
       	   min_out <= 0;
       	   sec_out <= 0;	   
       end else begin
       	   if (sec_out != 59) begin
       	      sec_out <= sec_out + 1;	
       	      if (min_pulse & ~mode) begin
       	         if (min_out != 59) min_out <= min_out + 1;
       	         else min_out <= 0;
       	      end
       	      if (hr_pulse & ~mode) begin
       	         if (hr_out != 12) hr_out <= hr_out + 1;
       	         else hr_out <= 1;
       	      end
       	   end else begin
       	      sec_out <= 0;
       	      if (min_out != 59) begin
       	         min_out <= min_out + 1;
       	         if (hr_pulse & ~mode) begin
       	            if (hr_out != 12) hr_out <= hr_out + 1;
       	            else hr_out <= 1;
       	         end
       	      end else begin
       	      	 min_out <= 0;
       	      	 if (hr_out != 12) hr_out <= hr_out + 1;
       	      	 else hr_out <= 1;
       	      end
       	   end
       	end

   always @(posedge clk or negedge rst_n)
       if (~rst_n) begin
       	  hr_alarm <= 12;
       	  min_alarm <= 0;
       end else if (set_alarm) begin
       	  if (hr_pulse) begin
       	     if (hr_alarm != 12) hr_alarm <= hr_alarm + 1;
       	     else hr_alarm <= 1;
       	  end
       	  if (min_pulse) begin
       	     if (min_alarm != 59) min_alarm <= min_alarm + 1;
       	     else min_alarm <= 0;
       	  end
       end
   
    always @(posedge clk or negedge rst_n)
       if (~rst_n) begin
       	  alarm <= 0;
       end else if ((hr_alarm == hr_out) && (min_alarm == min_out)) begin
          alarm <= 1;
       end else begin
          alarm <= 0;
       end        	      
endmodule

